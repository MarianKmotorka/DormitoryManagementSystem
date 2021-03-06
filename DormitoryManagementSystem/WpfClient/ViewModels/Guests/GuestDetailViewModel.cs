﻿using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using WpfClient.Events;
using WpfClient.ModelWrappers;

namespace WpfClient.ViewModels.Guests
{
    public class GuestDetailViewModel : Screen
    {
        private bool _isEditing;
        private bool _loading;
        private bool _editedSuccessfully;
        private readonly IGuestsEndpoint _guestsEndpoint;
        private readonly IEventAggregator _eventAggregator;

        public GuestModelWrapper Model { get; set; } = new GuestModelWrapper();

        public object GoBackViewModel { get; set; }

        public string GuestId { get; set; } = null;

        public bool IsMyInfoPage => GuestId == null;

        public bool IsEditButtonVisible => !IsEditing && !IsMyInfoPage;

        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; NotifyOfPropertyChange(nameof(IsEditing)); }
        }

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public bool EditedSuccessfully
        {
            get => _editedSuccessfully;
            set { _editedSuccessfully = value; NotifyOfPropertyChange(nameof(EditedSuccessfully)); }
        }

        public GuestDetailViewModel(IGuestsEndpoint guestsEndpoint, IEventAggregator eventAggregator)
        {
            _guestsEndpoint = guestsEndpoint;
            _eventAggregator = eventAggregator;
        }

        public void Edit()
        {
            IsEditing = true;
            EditedSuccessfully = false;
        }

        public async Task SubmitEdit()
        {
            EditedSuccessfully = false;

            if (!Model.ValidateModel(nameof(Model.Password)))
                return;

            Loading = true;

            var result = await _guestsEndpoint.Edit(GuestId, Model.Model);

            Loading = false;

            if (result.Fail)
            {
                Model.AddErrors(result.ErrorDetails);
                return;
            }

            EditedSuccessfully = true;
            IsEditing = false;
        }

        public void BackToGuests()
        {
            _ = GoBackViewModel ?? throw new ArgumentNullException(nameof(GoBackViewModel));

            _eventAggregator.PublishOnUIThread(new GoBackEvent(GoBackViewModel));
        }

        protected async override void OnViewLoaded(object view)
        {
            Loading = true;

            Model.Model = await _guestsEndpoint.GetDetail(GuestId);
            NotifyOfPropertyChange(nameof(Model));

            Loading = false;
        }
    }
}
