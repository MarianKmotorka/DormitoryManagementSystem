using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Library.Api.Interfaces;
using WpfClient.Events;

namespace WpfClient.ViewModels
{
    public class LogInViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IAppUsersEndpoint _appUsersEndpoint;
        private string _email = "admin@admin.com";
        private string _password = "admin123";
        private string _error;
        private bool _loading;
        private bool _success;
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

        public bool Success
        {
            get => _success;
            set { _success = value; NotifyOfPropertyChange(nameof(Success)); }
        }


        public LogInViewModel(IEventAggregator eventAggregator, IAppUsersEndpoint apiHelper)
        {
            _eventAggregator = eventAggregator;
            _appUsersEndpoint = apiHelper;
        }

        public async Task LogIn()
        {
            ResetBeforeRequest();

            var result = await _appUsersEndpoint.Authenticate(Email, Password);

            Loading = false;
            if (result.Fail)
            {
                Error = IoC.Get<ResourceDictionary>("language")[result.ErrorMessage].ToString();

                if (result.ErrorMessage == "EmailNotConfirmed")
                    NeedConfirmEmail = true;
                return;
            }

            _eventAggregator.PublishOnUIThread(new LoggedInEvent());
        }

        public async Task SendConfirmEmail()
        {
            ResetBeforeRequest();

            var result = await _appUsersEndpoint.ConfirmEmail(Email);

            Loading = false;

            if (result.Fail)
            {
                var error = result.ErrorDetails.First(x => x.PropertyName == "Email").Message;
                Error = IoC.Get<ResourceDictionary>("language")[error].ToString();
                return;
            }

            Success = true;
            NeedConfirmEmail = false;
        }

        public async Task SendResetPasswordEmail()
        {
            ResetBeforeRequest();

            var result = await _appUsersEndpoint.ResetPassword(Email);

            Loading = false;

            if (result.Fail)
            {
                var error = result.ErrorDetails.First(x => x.PropertyName == "Email").Message;
                Error = IoC.Get<ResourceDictionary>("language")[error].ToString();
                return;
            }

            Success = true;
        }

        public void Register()
        {
            _eventAggregator.PublishOnUIThread(new OpenRegisterGuestFormEvent());
        }

        private void ResetBeforeRequest()
        {
            Error = "";
            Success = false;
            Loading = true;
            NeedConfirmEmail = false;
        }
    }
}
