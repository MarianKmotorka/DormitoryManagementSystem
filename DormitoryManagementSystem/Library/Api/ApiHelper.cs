using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Library.Api.Interfaces;

namespace Library.Api
{
    public class ApiHelper : IApiHelper
    {
        public HttpClient Client { get; set; }

        public ApiHelper()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            var api = ConfigurationManager.AppSettings["api"];

            Client = new HttpClient();
            Client.BaseAddress = new Uri(api);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void LogOut()
        {
            Client.DefaultRequestHeaders.Remove("Authorization");
        }
    }
}
