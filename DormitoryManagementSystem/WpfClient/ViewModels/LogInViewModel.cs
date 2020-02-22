using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Library.Api.Interfaces;

namespace WpfClient.ViewModels
{
    public class LogInViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IAppUsersEndpoint _appUsersEndpoint;
        private string _email = "bob@guest.com";
        private string _password = "string";
        private string _error;
        private bool _loading;
        private bool _needConfirmEmail;

        public string Email
        {
            get => _email;
            set { _email = value; NotifyOfPropertyChange(nameof(Email)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; NotifyOfPropertyChange(nameof(Password)); }
        }

        public string Error
        {
            get => _error;
            set { _error = value; NotifyOfPropertyChange(nameof(Error)); }
        }

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public bool NeedConfirmEmail
        {
            get => _needConfirmEmail;
            set { _needConfirmEmail = value; NotifyOfPropertyChange(nameof(NeedConfirmEmail)); }
        }

        public LogInViewModel(IEventAggregator eventAggregator, IAppUsersEndpoint apiHelper)
        {
            _eventAggregator = eventAggregator;
            _appUsersEndpoint = apiHelper;
        }

        public async Task LogIn()
        {
            Error = "";
            Loading = true;
            NeedConfirmEmail = false;

            var result = await _appUsersEndpoint.Authenticate(Email, Password);

            Loading = false;
            if (result.Fail)
            {
                Error = IoC.Get<ResourceDictionary>("language")[result.Error].ToString();

                if (result.Error == "EmailNotConfirmed")
                    NeedConfirmEmail = true;
            }

            //TODO ACtiate other item
        }

        public async Task SendConfirmEmail()
        {
            Error = "";
            Loading = true;

            var result = await _appUsersEndpoint.ConfirmEmail(Email);

            Loading = false;

            if (result.Fail)
            {
                var error = result.Errors["Email"].First();
                Error = IoC.Get<ResourceDictionary>("language")[error].ToString();
                return;
            }

            NeedConfirmEmail = false;
        }
    }
}
