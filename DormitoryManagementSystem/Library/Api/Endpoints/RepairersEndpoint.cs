using System.Net.Http;
using System.Threading.Tasks;
using Library.Api.Interfaces;
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

        public async Task<PropertiesResultModel> Register(RepairerModel model)
        {
            var response = await _apiHelper.Client.PostAsJsonAsync("repairers", model);

            if (response.IsSuccessStatusCode)
                return PropertiesResultModel.Succesful;

            return await response.Content.ReadAsAsync<PropertiesResultModel>();
        }
    }
}
