using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Library.Api;
using Library.Api.Endpoints;
using Library.Api.Interfaces;
using Library.Models;
using WpfClient.Helpers;
using WpfClient.ViewModels;

namespace WpfClient
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
            LoadLanguage();

            ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");
        }

        protected override void Configure()
        {
            _container.Instance(_container);

            _container
            .Singleton<CurrentUser>()
            .Singleton<IApiHelper, ApiHelper>()
            .Singleton<IEventAggregator, EventAggregator>()
            .Singleton<IWindowManager, WindowManager>();

            _container.RegisterPerRequest(typeof(IAppUsersEndpoint), null, typeof(AppUsersEndpoint));
            _container.RegisterPerRequest(typeof(IGuestsEndpoint), null, typeof(GuestsEndpoint));
            _container.RegisterPerRequest(typeof(IOfficersEndpoint), null, typeof(OfficersEndpoint));

            GetType().Assembly.GetTypes()
                .Where(t => t.IsClass)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(t => _container.RegisterPerRequest(t, t.ToString(), t));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        private void LoadLanguage()
        {
            var dictionary = new ResourceDictionary
            {
                Source = new Uri("..\\Resources\\lang.en.xaml", UriKind.Relative)
            };

            _container.RegisterInstance(typeof(ResourceDictionary), "language", dictionary);

            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }
    }
}
