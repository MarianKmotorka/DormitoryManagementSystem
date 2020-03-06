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

        public async Task<PropertiesResultModel> ApproveAccomodationRequest(int requestId, int roomId, string additionalMessage)
        {
            var data = new
            {
                roomId,
                additionalMessage,
                isAccomodationRequestApproved = true
            };

            var response = await _apiHelper.Client.PatchAsJsonAsync($"accomodationRequests/{requestId}", data);

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<PropertiesResultModel>();

            return PropertiesResultModel.Succesful;
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

        public async Task<PropertiesResultModel> RejectAccomodationRequest(int requestId, string additionalMessage)
        {
            var data = new
            {
                additionalMessage,
                isAccomodationRequestApproved = false
            };

            var response = await _apiHelper.Client.PatchAsJsonAsync($"accomodationRequests/{requestId}", data);

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<PropertiesResultModel>();

            return PropertiesResultModel.Succesful;
        }

        public async Task<PropertiesResultModel> Create(NewAccomodationRequestModel model)
        {
            var response = await _apiHelper.Client.PostAsJsonAsync("accomodationRequests", model);

            if (response.IsSuccessStatusCode)
                return PropertiesResultModel.Succesful;

            return await response.Content.ReadAsAsync<PropertiesResultModel>();
        }
    }
}
