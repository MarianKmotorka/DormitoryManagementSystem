using System;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.RepairRequests;
using WpfClient.Events;

namespace WpfClient.ViewModels.RepairRequests
{
    public class RepairRequestDetailViewModel : Screen
    {
        private bool _loading;
        private readonly IRepairRequestsEndpoint _repairRequestsEndpoint;
        private readonly IEventAggregator _eventAggregator;
        private readonly IGuestsEndpoint _guestsEndpoint;

        public object GoBackViewModel { get; set; }

        public RepairRequestModel Model { get; set; } = new RepairRequestModel();

        public bool IsMyRepairRequest { get; set; }

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public RepairRequestDetailViewModel(IRepairRequestsEndpoint repairRequestsEndpoint,
            IEventAggregator eventAggregator, IGuestsEndpoint guestsEndpoint)
        {
            _repairRequestsEndpoint = repairRequestsEndpoint;
            _eventAggregator = eventAggregator;
            _guestsEndpoint = guestsEndpoint;
        }

        public void GoBack()
        {
            _ = GoBackViewModel ?? throw new ArgumentNullException(nameof(GoBackViewModel));

            _eventAggregator.PublishOnUIThread(new GoBackEvent(GoBackViewModel));
        }

        public void Respond()
        {
            _eventAggregator.PublishOnUIThread(new OpenRespondToRepairRequestViewEvent(this, Model.Id));
        }

        protected async override void OnViewLoaded(object view)
        {
            Loading = true;

            var result = IsMyRepairRequest
                ? await _guestsEndpoint.GetMyRepairRequestDetail(Model.Id)
                : await _repairRequestsEndpoint.GetDetail(Model.Id);

            Loading = false;

            Model = result;
            NotifyOfPropertyChange(nameof(Model));
        }
    }
}
