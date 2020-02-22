using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using WpfClient.Events;
using WpfClient.Models;

namespace WpfClient.ViewModels
{
    public class GuestRegistrationViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IGuestsEndpoint _guestsEndpoint;
        private bool _loading;

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public GuestRegistrationModelWrapper Model { get; set; } = new GuestRegistrationModelWrapper();

        public GuestRegistrationViewModel(IEventAggregator eventAggregator, IGuestsEndpoint guestsEndpoint)
        {
            _eventAggregator = eventAggregator;
            _guestsEndpoint = guestsEndpoint;
        }

        public async Task Register()
        {
            Loading = true;

            var result = await _guestsEndpoint.RegisterGuest(Model.Model);

            Loading = false;

            if (result.Fail)
            {
                foreach (var propErrors in result.Errors)
                    foreach (var error in propErrors.Value)
                        Model.AddError(propErrors.Key, error);

                return;
            }

            _eventAggregator.PublishOnUIThread(new GuestRegisteredEvent());
        }
    }
}
