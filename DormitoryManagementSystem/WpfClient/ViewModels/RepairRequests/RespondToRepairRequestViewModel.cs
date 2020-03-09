using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.RepairRequests;
using WpfClient.Events;
using WpfClient.ModelWrappers;

namespace WpfClient.ViewModels.RepairRequests
{
    public class RespondToRepairRequestViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRepairRequestsEndpoint _repairRequestsEndpoint;
        private bool _loading;

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public object GoBackViewModel { get; set; }

        public int RequestId { get; set; }

        public bool IsWillBeFixedOnVisible => Model.RepairRequestState == RepairRequestState.Accepted;

        public IEnumerable<RepairRequestState> RequestStates =>
            new[]
            {
                RepairRequestState.Accepted,
                RepairRequestState.Fixed,
                RepairRequestState.Refused
            };

        public RespondToRepairRequestModelWrapper Model { get; set; } = new RespondToRepairRequestModelWrapper();

        public RespondToRepairRequestViewModel(IEventAggregator eventAggregator, IRepairRequestsEndpoint repairRequestsEndpoint)
        {
            _eventAggregator = eventAggregator;
            _repairRequestsEndpoint = repairRequestsEndpoint;
        }

        public void GoBack()
        {
            _ = GoBackViewModel ?? throw new ArgumentNullException(nameof(GoBackViewModel));

            _eventAggregator.PublishOnUIThread(new GoBackEvent(GoBackViewModel));
        }

        public async Task Submit()
        {
            Model.ClearAllErrors();

            Loading = true;

            var result = await _repairRequestsEndpoint.RespondToRepairRequest(RequestId, Model.Model);

            Loading = false;

            if (result.Fail)
            {
                foreach (var propError in result.Errors)
                    foreach (var error in propError.Value)
                        Model.AddError(propError.Key, error);

                return;
            }

            GoBack();
        }

        public void SelectedStateChanged()
        {
            NotifyOfPropertyChange(nameof(IsWillBeFixedOnVisible));
        }

        protected override void OnViewLoaded(object view)
        {
            var detail = GoBackViewModel as RepairRequestDetailViewModel;

            Model.WillBeFixedOn = detail?.Model.WillBeFixedOn ?? DateTime.UtcNow;
            Model.RepairerReply = detail?.Model.RepairerReply ?? string.Empty;
            Model.RepairRequestState = detail?.Model.State == null || detail.Model.State == RepairRequestState.Pending
                ? RepairRequestState.Accepted
                : detail.Model.State;

            SelectedStateChanged();
        }
    }
}
