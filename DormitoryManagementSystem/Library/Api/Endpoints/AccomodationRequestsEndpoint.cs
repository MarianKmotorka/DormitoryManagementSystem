using System.Net.Http;
using System.Threading.Tasks;
using Library.Api.Interfaces;
using Library.Api.Utils;
using Library.Models;
using Library.Models.AccomodationRequests;

namespace Library.Api.Endpoints
{
    public class AccomodationRequestsEndpoint : IAccomodationRequestsEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public AccomodationRequestsEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<PagedResultModel<AccomodationRequestLookup>> GetAll(PagedRequestModel model)
        {
            var url = UrlBuilder.Build("accomodationRequests", model);

            var response = await _apiHelper.Client.GetAsync(url);

            return await response.Content.ReadAsAsync<PagedResultModel<AccomodationRequestLookup>>();
        }

        public async Task<AccomodationRequestDetail> GetDetail(int id)
        {
            var response = await _apiHelper.Client.GetAsync($"accomodationRequests/{id}");

            return await response.Content.ReadAsAsync<AccomodationRequestDetail>();
        }
    }
}
