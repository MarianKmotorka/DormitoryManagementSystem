using System;
using System.Net.Http;
using System.Threading.Tasks;
using Library.Api.Interfaces;
using Library.Models;
using Library.Models.Guests;

namespace Library.Api
{
    public class GuestsEndpoint : IGuestsEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public GuestsEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<PropertiesResultModel> RegisterGuest(GuestModel model)
        {
            var response = await _apiHelper.Client.PostAsJsonAsync("guests", model);

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<PropertiesResultModel>();

            return PropertiesResultModel.Succesful;
        }

        public async Task<GuestModel> GetGuestDetail(string id = null)
        {
            var response = await _apiHelper.Client.GetAsync($"guests/{id ?? "me"}");

            return await response.Content.ReadAsAsync<GuestModel>();
        }

        public async Task<PropertiesResultModel> EditGuest(string id, GuestModel model)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Id cannot be null");

            var response = await _apiHelper.Client.PatchAsJsonAsync($"guests/{id}", model);

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<PropertiesResultModel>();

            return PropertiesResultModel.Succesful;
        }
    }
}
