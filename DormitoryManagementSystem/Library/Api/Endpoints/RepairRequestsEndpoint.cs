﻿using System.Net.Http;
using System.Threading.Tasks;
using Library.Api.Interfaces;
using Library.Api.Utils;
using Library.Models;
using Library.Models.RepairRequests;

namespace Library.Api.Endpoints
{
    public class RepairRequestsEndpoint : IRepairRequestsEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public RepairRequestsEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<PropertiesResultModel> Create(NewRepairRequestModel model)
        {
            var data = new
            {
                ProblemDescription = model.ProblemDescription ?? "",
                RoomItemTypeId = model.RoomItemType?.Id ?? -1
            };

            var response = await _apiHelper.Client.PostAsJsonAsync("repairRequests", data);

            if (response.IsSuccessStatusCode)
                return PropertiesResultModel.Succesful;

            return await response.Content.ReadAsAsync<PropertiesResultModel>();
        }

        public async Task<PagedResultModel<RepairRequestLookup>> GetAll(PagedRequestModel model)
        {
            var url = UrlBuilder.Build("repairRequests", model);

            var response = await _apiHelper.Client.GetAsync(url);

            return await response.Content.ReadAsAsync<PagedResultModel<RepairRequestLookup>>();
        }
    }
}
