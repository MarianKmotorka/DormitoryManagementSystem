using System.Net.Http;
using System.Threading.Tasks;
using Library.Api.Interfaces;
using Library.Api.Utils;
using Library.Models;
using Library.Models.Rooms;

namespace Library.Api.Endpoints
{
    public class RoomsEndpoint : IRoomsEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public RoomsEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<PagedResultModel<RoomLookup>> GetAll(PagedRequestModel model)
        {
            var url = UrlBuilder.Build("rooms", model);

            var response = await _apiHelper.Client.GetAsync(url);

            return await response.Content.ReadAsAsync<PagedResultModel<RoomLookup>>();
        }

        public async Task<RoomModel> GetDetail(int id)
        {
            var response = await _apiHelper.Client.GetAsync($"rooms/{id}");

            return await response.Content.ReadAsAsync<RoomModel>();
        }
    }
}
