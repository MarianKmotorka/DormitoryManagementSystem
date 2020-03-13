using System;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.Offices;
using WpfClient.Events;

namespace WpfClient.ViewModels.Offices
{
    public class OfficeDetailViewModel : Screen
    {
        private bool _loading;
        private readonly IEventAggregator _eventAggregator;
        private readonly IOfficesEndpoint _officesEndpoint;

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public object GoBackViewModel { get; set; }

        public OfficeModel Model { get; set; } = new OfficeModel();

        public OfficeDetailViewModel(IEventAggregator eventAggregator, IOfficesEndpoint officesEndpoint)
        {
            _eventAggregator = eventAggregator;
            _officesEndpoint = officesEndpoint;
        }

        public void GoBack()
        {
            _ = GoBackViewModel ?? throw new ArgumentNullException(nameof(GoBackViewModel));

            _eventAggregator.PublishOnUIThread(new GoBackEvent(GoBackViewModel));
        }

        public void OpenOfficerDetail(string officerId)
        {
            _eventAggregator.PublishOnUIThread(new OpenOfficerDetailEvent(this, officerId));
        }

        protected async override void OnViewLoaded(object view)
        {
            Loading = true;
            Model = await _officesEndpoint.GetDetail(Model.Id);
            Loading = false;

            NotifyOfPropertyChange(nameof(Model));
        }
    }
}
