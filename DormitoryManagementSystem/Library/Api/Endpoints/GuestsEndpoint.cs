using System;
using System.Net.Http;
using System.Threading.Tasks;
using Library.Api.Interfaces;
using Library.Api.Utils;
using Library.Models;
using Library.Models.AccomodationRequests;
using Library.Models.Guests;

namespace Library.Api.Endpoints
{
    public class GuestsEndpoint : IGuestsEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public GuestsEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<PropertiesResultModel> Register(GuestModel model)
        {
            var response = await _apiHelper.Client.PostAsJsonAsync("guests", model);

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<PropertiesResultModel>();

            return PropertiesResultModel.Succesful;
        }

        public async Task<GuestModel> GetDetail(string id = null)
        {
            var response = await _apiHelper.Client.GetAsync($"guests/{id ?? "me"}");

            return await response.Content.ReadAsAsync<GuestModel>();
        }

        public async Task<PagedResultModel<GuestLookup>> GetAll(PagedRequestModel model)
        {
            var url = UrlBuilder.Build("guests", model);

            var response = await _apiHelper.Client.GetAsync(url);

            return await response.Content.ReadAsAsync<PagedResultModel<GuestLookup>>();
        }

        public async Task<PropertiesResultModel> Edit(string id, GuestModel model)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Id cannot be null");

            var response = await _apiHelper.Client.PatchAsJsonAsync($"guests/{id}", model);

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<PropertiesResultModel>();

            return PropertiesResultModel.Succesful;
        }

        public async Task<PagedResultModel<AccomodationRequestLookup>> GetMyAccomodationRequests(PagedRequestModel model)
        {
            var url = UrlBuilder.Build("guests/me/accomodation-requests", model);

            var response = await _apiHelper.Client.GetAsync(url);

            return await response.Content.ReadAsAsync<PagedResultModel<AccomodationRequestLookup>>();
        }
    }
}
