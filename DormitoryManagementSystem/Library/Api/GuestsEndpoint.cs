using System.Net.Http;
using System.Threading.Tasks;
using Library.Api.Interfaces;
using Library.Models;

namespace Library.Api
{
    public class GuestsEndpoint : IGuestsEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public GuestsEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<PropertiesResultModel> RegisterGuest(GuestRegistrationModel model)
        {
            var response = await _apiHelper.Client.PostAsJsonAsync("guests", model);

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<PropertiesResultModel>();

            return PropertiesResultModel.Succesful;
        }
    }
}
