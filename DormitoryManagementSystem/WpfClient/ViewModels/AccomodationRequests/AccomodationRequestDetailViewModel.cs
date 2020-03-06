using System;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.AccomodationRequests;
using WpfClient.Events;

namespace WpfClient.ViewModels.AccomodationRequests
{
    public class AccomodationRequestDetailViewModel : Screen
    {
        private bool _loading;
        private readonly IAccomodationRequestsEndpoint _accomodationRequestsEndpoint;
        private readonly IEventAggregator _eventAggregator;

        public AccomodationRequestListViewModel GoBackViewModel { get; set; }

        public AccomodationRequestDetail Model { get; set; } = new AccomodationRequestDetail();

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public AccomodationRequestDetailViewModel(IAccomodationRequestsEndpoint accomodationRequestsEndpoint,
            IEventAggregator eventAggregator)
        {
            _accomodationRequestsEndpoint = accomodationRequestsEndpoint;
            _eventAggregator = eventAggregator;
        }

        public void BackToAccomodationRequests()
        {
            _ = GoBackViewModel ?? throw new ArgumentNullException(nameof(GoBackViewModel));

            _eventAggregator.PublishOnUIThread(new GoBackEvent(GoBackViewModel));
        }

        public void Respond()
        {
            _eventAggregator.PublishOnUIThread(new OpenRespondToAccomodationRequestViewEvent(this, Model.Id));
        }

        protected async override void OnViewLoaded(object view)
        {
            Loading = true;

            var result = await _accomodationRequestsEndpoint.GetDetail(Model.Id);

            Loading = false;

            Model = result;
            NotifyOfPropertyChange(nameof(Model));
        }
    }
}
