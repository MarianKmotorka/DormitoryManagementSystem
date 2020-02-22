using System;
using System.Windows;
using Caliburn.Micro;
using Library.Api.Interfaces;
using WpfClient.Events;

namespace WpfClient.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<OpenRegisterGuestFormEvent>, IHandle<LoggedInEvent>,
        IHandle<GuestRegisteredEvent>
    {
        private readonly SimpleContainer _simpleContainer;
        private readonly IApiHelper _apiHelper;
        private bool _isEnglish = true;
        private bool _isSlovak = false;
        private bool _isLoggedIn = false;

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

        public ShellViewModel(IEventAggregator eventAggregator, SimpleContainer simpleContainer,
            IApiHelper apiHelper)
        {
            _simpleContainer = simpleContainer;
            _apiHelper = apiHelper;
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

        public void Handle(OpenRegisterGuestFormEvent message)
        {
            ActivateItem(IoC.Get<GuestRegistrationViewModel>());
        }

        public void Handle(LoggedInEvent message)
        {
            IsLoggedIn = true;
        }

        public void Handle(GuestRegisteredEvent message)
        {
            ActivateItem(IoC.Get<LogInViewModel>());
        }
    }
}
