using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using WpfClient.Events;
using WpfClient.ModelWrappers;

namespace WpfClient.ViewModels.AccomodationRequests
{
    public class NewAccomodationRequestViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IAccomodationRequestsEndpoint _accomodationRequestsEndpoint;
        private bool _loading;

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public object GoBackViewModel { get; set; }

        public NewAccomodationRequestModelWrapper Model { get; set; } = new NewAccomodationRequestModelWrapper();

        public NewAccomodationRequestViewModel(IEventAggregator eventAggregator, IAccomodationRequestsEndpoint accomodationRequestsEndpoint)
        {
            _eventAggregator = eventAggregator;
            _accomodationRequestsEndpoint = accomodationRequestsEndpoint;
        }

        public async Task Submit()
        {
            Model.ClearAllErrors();

            Loading = true;

            var result = await _accomodationRequestsEndpoint.Create(Model.Model);

            Loading = false;

            if (result.Fail)
            {
                Model.AddErrors(result.ErrorDetails);
                return;
            }

            GoBack();
        }

        public void GoBack()
        {
            _ = GoBackViewModel ?? throw new ArgumentNullException(nameof(GoBackViewModel));

            _eventAggregator.PublishOnUIThread(new GoBackEvent(GoBackViewModel));
        }
    }
}
