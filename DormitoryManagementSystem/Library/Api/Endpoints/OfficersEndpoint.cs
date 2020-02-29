using System;
using System.Net.Http;
using System.Threading.Tasks;
using Library.Api.Interfaces;
using Library.Api.Utils;
using Library.Models;
using Library.Models.Officers;

namespace Library.Api.Endpoints
{
    public class OfficersEndpoint : IOfficersEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public OfficersEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<PropertiesResultModel> Edit(string id, OfficerModel model)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Id cannot be null");

            var response = await _apiHelper.Client.PatchAsJsonAsync($"officers/{id}", model);

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<PropertiesResultModel>();

            return PropertiesResultModel.Succesful;
        }

        public async Task<OfficerModel> GetDetail(string id = null)
        {
            var response = await _apiHelper.Client.GetAsync($"officers/{id ?? "me"}");

            return await response.Content.ReadAsAsync<OfficerModel>();
        }

        public Task<PropertiesResultModel> Register(OfficerModel model)
        {
            throw new NotImplementedException();
        }
    }
}
