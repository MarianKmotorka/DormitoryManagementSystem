using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Library.Api.Interfaces;
using Library.Models;
using Library.Models.Identity;
using Library.Models.Users;
using WpfClient.Validation;

namespace WpfClient.ViewModels.Admin
{
    public class ManageUsersPasswordsViewModel : ValidationWrapper<ChangePasswordByAdminModel>
    {
        private readonly IAppUsersEndpoint _appUsersEndpoint;

        private bool _loading;
        private bool _success;

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

        public string SearchText { get; set; } = "";

        public string NewPassword { get => GetValue<string>(); set => SetValue(value); }

        public ObservableCollection<UserLookup> Users { get; set; } = new ObservableCollection<UserLookup>();

        public UserLookup User { get => GetValue<UserLookup>(); set => SetValue(value); }

        public ManageUsersPasswordsViewModel(IAppUsersEndpoint appUsersEndpoint) : base(new ChangePasswordByAdminModel())
        {
            _appUsersEndpoint = appUsersEndpoint;
        }

        public async Task LoadUsers()
        {
            if (User?.Email == SearchText)
                return;

            Loading = true;

            var model = new PagedRequestModel
            {
                Sorts = new List<string> { "Email" },
                Filters = new List<string> { $"Email@={SearchText}" },
                PageSize = 7
            };

            var result = await _appUsersEndpoint.GetAll(model);

            Users.Clear();
            foreach (var user in result.Data)
            {
                Users.Add(user);
            }

            Loading = false;
        }

        public async Task Submit()
        {
            if (!ValidateModel())
                return;

            Success = false;
            Loading = true;
            var result = await _appUsersEndpoint.ChangePasswordByAdmin(User.Id, NewPassword);
            Loading = false;

            if (result.Fail)
            {
                AddErrors(result.ErrorDetails);
                return;
            }

            Success = true;
        }

        protected async override void OnViewLoaded(object view) => await LoadUsers();
    }
}
