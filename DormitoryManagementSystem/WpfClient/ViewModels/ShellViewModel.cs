using System;
using System.Windows;
using Caliburn.Micro;

namespace WpfClient.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly SimpleContainer _simpleContainer;
        private readonly ResourceDictionary _language;
        private bool _isEnglish = true;
        private bool _isSlovak = false;

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


        public ShellViewModel(IEventAggregator eventAggregator, SimpleContainer simpleContainer)
        {
            _eventAggregator = eventAggregator;
            _simpleContainer = simpleContainer;
            _language = IoC.Get<ResourceDictionary>("language");
        }

        public void ChangeLanguage(string language)
        {
            _simpleContainer.UnregisterHandler<ResourceDictionary>("language");
            Application.Current.Resources.MergedDictionaries.Remove(_language);

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
    }
}
