using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Library.Api.Interfaces;
using Library.Models.Identity;

namespace Library.Api
{
    public class ApiHelper : IApiHelper
    {
        private readonly CurrentUser _currentUser;

        public HttpClient Client { get; set; }

        public ApiHelper(CurrentUser currentUser)
        {
            _currentUser = currentUser;
            InitializeClient();
        }

        private void InitializeClient()
        {
            var api = ConfigurationManager.AppSettings["api"];

            Client = HttpClientFactory.Create(new AuthenticationHandler(_currentUser));
            Client.BaseAddress = new Uri(api);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void LogOut()
        {
            _currentUser.Jwt = "";
            _currentUser.RefreshToken = "";
        }
    }
}
