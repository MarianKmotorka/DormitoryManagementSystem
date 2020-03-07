using System;
using System.Windows;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.Identity;
using WpfClient.Events;
using WpfClient.ViewModels.AccomodationRequests;
using WpfClient.ViewModels.Admin;
using WpfClient.ViewModels.Guests;
using WpfClient.ViewModels.Officers;

namespace WpfClient.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandleShellEvents
    {
        private readonly SimpleContainer _simpleContainer;
        private readonly IApiHelper _apiHelper;
        private readonly CurrentUser _currentUser;
        private bool _isEnglish = true;
        private bool _isSlovak = false;
        private bool _isLoggedIn = false;

        #region Tabs Visibility Properties

        public bool OfficersVisible => _currentUser.Role == RoleNames.SysAdmin;

        public bool RegisterOfficerVisible => _currentUser.Role == RoleNames.SysAdmin;

        public bool RepairersVisible => _currentUser.Role == RoleNames.SysAdmin || _currentUser.Role == RoleNames.Officer;

        public bool GuestsVisible => _currentUser.Role == RoleNames.SysAdmin || _currentUser.Role == RoleNames.Officer;

        public bool AccomodationRequestsVisible => _currentUser.Role != RoleNames.Repairer;

        public bool RegisterRepairerVisible => _currentUser.Role == RoleNames.SysAdmin || _currentUser.Role == RoleNames.Officer;

        public bool RoomsVisible => _currentUser.Role == RoleNames.SysAdmin || _currentUser.Role == RoleNames.Officer;

        public bool OfficesVisible => _currentUser.Role == RoleNames.SysAdmin || _currentUser.Role == RoleNames.Officer;

        public bool MyRoomVisible => _currentUser.Role == RoleNames.Guest;

        public bool RepairRequestsVisible => _currentUser.Role == RoleNames.Guest || _currentUser.Role == RoleNames.Repairer;

        #endregion

        public bool IsEnglish
        {
            get => _isEnglish;
            set { _isEnglish = value; NotifyOfPropertyChange(nameof(IsEnglish)); }
        }

        public bool IsSlovak
        {
            get => _isSlovak;
            set { _isSlovak = value; NotifyOfPropertyChange(nameof(IsSlovak)); }
        }

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set { _isLoggedIn = value; NotifyOfPropertyChange(nameof(IsLoggedIn)); }
        }

        public string UserName { get; set; }

        public ShellViewModel(IEventAggregator eventAggregator, SimpleContainer simpleContainer,
            IApiHelper apiHelper, CurrentUser currentUser)
        {
            _simpleContainer = simpleContainer;
            _apiHelper = apiHelper;
            _currentUser = currentUser;

            eventAggregator.Subscribe(this);
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ActivateItem(IoC.Get<LogInViewModel>());
        }

        public void ChangeLanguage(string language)
        {
            _simpleContainer.UnregisterHandler<ResourceDictionary>("language");
            Application.Current.Resources.MergedDictionaries.Remove(IoC.Get<ResourceDictionary>("language"));

            var dictionary = new ResourceDictionary
            {
                Source = new Uri($"..\\Resources\\lang.{language}.xaml", UriKind.Relative)
            };

            _simpleContainer.RegisterInstance(typeof(ResourceDictionary), "language", dictionary);
            Application.Current.Resources.MergedDictionaries.Add(dictionary);

            if (language == "sk")
                IsEnglish = false;
            else
                IsSlovak = false;
        }

        public void OpenTab(string name)
        {
            switch (name)
            {
                case "MyInfo":
                    switch (_currentUser.Role)
                    {
                        case RoleNames.Guest:
                            ActivateItem(IoC.Get<GuestDetailViewModel>());
                            break;
                        case RoleNames.Officer:
                            ActivateItem(IoC.Get<OfficerDetailViewModel>());
                            break;
                        case RoleNames.SysAdmin:
                            ActivateItem(IoC.Get<AdminInfoViewModel>());
                            break;
                        case RoleNames.Repairer:
                            ActivateItem(null);
                            break;
                    }
                    break;

                case "Guests":
                    ActivateItem(IoC.Get<GuestListViewModel>());
                    break;

                case "RegisterOfficer":
                    ActivateItem(IoC.Get<OfficerRegistrationViewModel>());
                    break;

                case "AccomodationRequests":
                    var vm = IoC.Get<AccomodationRequestListViewModel>();
                    if (_currentUser.Role == RoleNames.Guest)
                        vm.IsMyAccomodationRequests = true;
                    ActivateItem(vm);
                    break;
            }
        }

        public void LogIn()
        {
            ActivateItem(IoC.Get<LogInViewModel>());
        }

        public void LogOut()
        {
            _apiHelper.LogOut();
            IsLoggedIn = false;
            ActivateItem(IoC.Get<LogInViewModel>());
        }

        public void ChangePassword()
        {
            ActivateItem(IoC.Get<ChangePasswordViewModel>());
        }

        public void Handle(OpenRegisterGuestFormEvent message)
        {
            ActivateItem(IoC.Get<GuestRegistrationViewModel>());
        }

        public void Handle(LoggedInEvent message)
        {
            IsLoggedIn = true;

            OpenTab("MyInfo");

            UserName = _currentUser.UserName;

            NotifyOfPropertyChange(nameof(OfficersVisible));
            NotifyOfPropertyChange(nameof(RegisterOfficerVisible));
            NotifyOfPropertyChange(nameof(RepairersVisible));
            NotifyOfPropertyChange(nameof(GuestsVisible));
            NotifyOfPropertyChange(nameof(AccomodationRequestsVisible));
            NotifyOfPropertyChange(nameof(RegisterRepairerVisible));
            NotifyOfPropertyChange(nameof(RoomsVisible));
            NotifyOfPropertyChange(nameof(MyRoomVisible));
            NotifyOfPropertyChange(nameof(RepairRequestsVisible));
            NotifyOfPropertyChange(nameof(OfficesVisible));
            NotifyOfPropertyChange(nameof(UserName));
        }

        public void Handle(GuestRegisteredEvent message)
        {
            ActivateItem(IoC.Get<LogInViewModel>());
        }

        public void Handle(OpenGuestDetailEvent message)
        {
            var vm = IoC.Get<GuestDetailViewModel>();
            vm.GuestId = message.GuestId;
            vm.GoBackViewModel = message.Sender as GuestListViewModel;
            ActivateItem(vm);
        }

        public void Handle(GoBackEvent message)
        {
            ActivateItem(message.ViewModel);
        }

        public void Handle(OpenAccomodationRequestDetailEvent message)
        {
            var vm = IoC.Get<AccomodationRequestDetailViewModel>();
            vm.Model.Id = message.Id;
            vm.GoBackViewModel = message.Sender as AccomodationRequestListViewModel;
            ActivateItem(vm);
        }

        public void Handle(OpenRespondToAccomodationRequestViewEvent message)
        {
            var vm = IoC.Get<RespondToAccomodationRequestViewModel>();
            vm.RequestId = message.RequestId;
            vm.GoBackViewModel = message.Sender;
            ActivateItem(vm);
        }

        public void Handle(OpenNewAccomodationRequestViewEvent message)
        {
            var vm = IoC.Get<NewAccomodationRequestViewModel>();
            vm.GoBackViewModel = message.Sender;
            ActivateItem(vm);
        }
    }
}
