using System;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.Identity;
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
        private readonly CurrentUser _currentUser;

        public object GoBackViewModel { get; set; }

        public RepairRequestModel Model { get; set; } = new RepairRequestModel();

        public bool CanRespond => _currentUser.Role == RoleNames.Repairer && Model.State != RepairRequestState.Fixed;

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public RepairRequestDetailViewModel(IRepairRequestsEndpoint repairRequestsEndpoint,
            IEventAggregator eventAggregator, IGuestsEndpoint guestsEndpoint, CurrentUser currentUser)
        {
            _repairRequestsEndpoint = repairRequestsEndpoint;
            _eventAggregator = eventAggregator;
            _guestsEndpoint = guestsEndpoint;
            _currentUser = currentUser;
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

            var result = _currentUser.Role == RoleNames.Guest
                ? await _guestsEndpoint.GetMyRepairRequestDetail(Model.Id)
                : await _repairRequestsEndpoint.GetDetail(Model.Id);

            Loading = false;

            Model = result;
            NotifyOfPropertyChange(nameof(Model));
            NotifyOfPropertyChange(nameof(CanRespond));
        }
    }
}
