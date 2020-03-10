using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Library.Api.Interfaces;
using Library.Models;
using Library.Models.Identity;
using Newtonsoft.Json;

namespace Library.Api.Endpoints
{
    public class AppUsersEndpoint : IAppUsersEndpoint
    {
        private readonly IApiHelper _apiHelper;
        private readonly CurrentUser _currentUser;

        public AppUsersEndpoint(IApiHelper apiHelper, CurrentUser currentUser)
        {
            _apiHelper = apiHelper;
            _currentUser = currentUser;
        }

        public async Task<ResultModel> Authenticate(string email, string password)
        {
            var body = new { email, password };

            var response = await _apiHelper.Client.PostAsJsonAsync("appUsers/login", body);

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ResultModel>();

            var currentUser = await response.Content.ReadAsAsync<CurrentUser>();

            _currentUser.Jwt = currentUser.Jwt;
            _currentUser.Role = currentUser.Role;
            _currentUser.RefreshToken = currentUser.RefreshToken;
            _currentUser.UserName = currentUser.UserName;

            return ResultModel.Successful;
        }

        public async Task<ResultModel> ConfirmEmail(string email)
        {
            var response = await _apiHelper.Client.GetAsync($"appUsers/confirmation-email?email={email}");

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ResultModel>();

            return ResultModel.Successful;
        }

        public async Task<ResultModel> ResetPassword(string email)
        {
            var response = await _apiHelper.Client.GetAsync($"appUsers/forgotten-password?email={email}");

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ResultModel>();

            return ResultModel.Successful;
        }

        public async Task<ResultModel> ChangePassword(ChangePasswordModel model)
        {
            var body = new { model.CurrentPassword, model.NewPassword };

            var response = await _apiHelper.Client.PostAsJsonAsync("appUsers/password", body);

            if (response.IsSuccessStatusCode)
                return ResultModel.Successful;

            return await response.Content.ReadAsAsync<ResultModel>();
        }

        public static HttpRequestMessage GetRefreshTokenHttpRequestMessage(CurrentUser currentUser)
        {
            var body = new
            {
                ExpiredJwt = currentUser.Jwt,
                currentUser.RefreshToken
            };

            var url = $"{ConfigurationManager.AppSettings["api"]}appUsers/refresh";

            var httpRequestMessage = new HttpRequestMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"),
                Method = new HttpMethod("POST"),
                RequestUri = new Uri(url)
            };

            return httpRequestMessage;
        }
    }
}
