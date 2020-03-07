using System.Threading.Tasks;
using Library.Api.Interfaces;
using Library.Models.Identity;
using WpfClient.Validation;

namespace WpfClient.ViewModels
{
    public class ChangePasswordViewModel : ValidationWrapper<ChangePasswordModel>
    {
        private bool _loading;
        private bool _success;
        private readonly IAppUsersEndpoint _appUsersEndpoint;

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public bool Success
        {
            get => _success;
            set { _success = value; NotifyOfPropertyChange(nameof(Success)); }
        }

        public string CurrentPassword
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string NewPassword
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ChangePasswordViewModel(IAppUsersEndpoint appUsersEndpoint) : base(new ChangePasswordModel())
        {
            _appUsersEndpoint = appUsersEndpoint;
        }

        public async Task Submit()
        {
            Success = false;

            if (!ValidateModel())
                return;

            Loading = true;

            var result = await _appUsersEndpoint.ChangePassword(Model);

            Loading = false;

            if (result.Fail)
            {
                foreach (var propError in result.Errors)
                    foreach (var error in propError.Value)
                        AddError(propError.Key, error);

                return;
            }

            Success = true;
        }
    }
}
