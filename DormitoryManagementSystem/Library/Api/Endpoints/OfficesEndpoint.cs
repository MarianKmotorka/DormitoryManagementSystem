using System.Net.Http;
using System.Threading.Tasks;
using Library.Api.Interfaces;
using Library.Api.Utils;
using Library.Models;
using Library.Models.Offices;

namespace Library.Api.Endpoints
{
    public class OfficesEndpoint : IOfficesEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public OfficesEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<PagedResultModel<OfficeLookup>> GetAll(PagedRequestModel model)
        {
            var url = UrlBuilder.Build("offices", model);

            var response = await _apiHelper.Client.GetAsync(url);

            return await response.Content.ReadAsAsync<PagedResultModel<OfficeLookup>>();
        }

        public async Task<OfficeModel> GetDetail(int id)
        {
            var response = await _apiHelper.Client.GetAsync($"offices/{id}");

            return await response.Content.ReadAsAsync<OfficeModel>();
        }
    }
}
