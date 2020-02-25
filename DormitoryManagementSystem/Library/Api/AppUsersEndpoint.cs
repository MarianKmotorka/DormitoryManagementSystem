using System.Net.Http;
using System.Threading.Tasks;
using Library.Api.Interfaces;
using Library.Models;

namespace Library.Api
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

            _apiHelper.Client.DefaultRequestHeaders.Add("Authorization", $"bearer {currentUser.Jwt}");

            return ResultModel.Successful;
        }

        public async Task<PropertiesResultModel> ConfirmEmail(string email)
        {
            var response = await _apiHelper.Client.GetAsync($"appUsers/confirmation-email?email={email}");

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<PropertiesResultModel>();

            return PropertiesResultModel.Succesful;
        }

        public async Task<PropertiesResultModel> ResetPassword(string email)
        {
            var response = await _apiHelper.Client.GetAsync($"appUsers/forgotten-password?email={email}");

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<PropertiesResultModel>();

            return PropertiesResultModel.Succesful;
        }
    }
}
