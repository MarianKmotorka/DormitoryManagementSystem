using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using WpfClient.Events;
using WpfClient.ModelWrappers;

namespace WpfClient.ViewModels.Officers
{
    public class OfficerDetailViewModel : Screen
    {
        private bool _isEditing;
        private bool _loading;
        private bool _editedSuccessfully;
        private readonly IOfficersEndpoint _officersEndpoint;
        private readonly IEventAggregator _eventAggregator;

        public OfficerModelWrapper Model { get; set; } = new OfficerModelWrapper();

        public object GoBackViewModel { get; set; }

        public string OfficerId { get; set; } = null;

        public bool IsMyInfoPage => OfficerId == null;

        public bool IsEditButtonVisible => !IsEditing && OfficerId != null;

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

        public OfficerDetailViewModel(IOfficersEndpoint officersEndpoint, IEventAggregator eventAggregator)
        {
            _officersEndpoint = officersEndpoint;
            _eventAggregator = eventAggregator;
        }

        public void Edit()
        {
            IsEditing = true;
            EditedSuccessfully = false;
        }

        public void GoBack()
        {
            _ = GoBackViewModel ?? throw new ArgumentNullException(nameof(GoBackViewModel));

            _eventAggregator.PublishOnUIThread(new GoBackEvent(GoBackViewModel));
        }

        public async Task SubmitEdit()
        {
            EditedSuccessfully = false;

            if (!Model.ValidateModel())
                return;

            Loading = true;

            var result = await _officersEndpoint.Edit(OfficerId, Model.Model);

            Loading = false;

            if (result.Fail)
            {
                Model.AddErrors(result.ErrorDetails);
                return;
            }

            EditedSuccessfully = true;
            IsEditing = false;
        }

        protected async override void OnViewLoaded(object view)
        {
            Loading = true;

            Model.Model = await _officersEndpoint.GetDetail(OfficerId);
            NotifyOfPropertyChange(nameof(Model));

            Loading = false;
        }
    }
}
