using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
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
        }

        protected override void Configure()
        {
            _container.Instance(_container);

            _container
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IWindowManager, WindowManager>();

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
