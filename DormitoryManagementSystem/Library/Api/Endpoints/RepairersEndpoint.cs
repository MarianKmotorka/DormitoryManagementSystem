using System.Net.Http;
using System.Threading.Tasks;
using Library.Api.Interfaces;
using Library.Api.Utils;
using Library.Models;
using Library.Models.Repairers;

namespace Library.Api.Endpoints
{
    public class RepairersEndpoint : IRepairersEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public RepairersEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<ResultModel> Edit(string id, RepairerModel model)
        {
            var response = await _apiHelper.Client.PatchAsJsonAsync($"repairers/{id}", model);

            if (response.IsSuccessStatusCode)
                return ResultModel.Successful;

            return await response.Content.ReadAsAsync<ResultModel>();
        }

        public async Task<PagedResultModel<RepairerLookup>> GetAll(PagedRequestModel model)
        {
            var url = UrlBuilder.Build("repairers", model);

            var response = await _apiHelper.Client.GetAsync(url);

            return await response.Content.ReadAsAsync<PagedResultModel<RepairerLookup>>();
        }

        public async Task<RepairerModel> GetDetail(string id = null)
        {
            var response = await _apiHelper.Client.GetAsync($"repairers/{id ?? "me"}");

            return await response.Content.ReadAsAsync<RepairerModel>();
        }

        public async Task<ResultModel> Register(RepairerModel model)
        {
            var response = await _apiHelper.Client.PostAsJsonAsync("repairers", model);

            if (response.IsSuccessStatusCode)
                return ResultModel.Successful;

            return await response.Content.ReadAsAsync<ResultModel>();
        }
    }
}
