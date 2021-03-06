﻿using System.Threading.Tasks;
using Library.Models;
using Library.Models.Officers;

namespace Library.Api.Interfaces
{
    public interface IOfficersEndpoint
    {
        Task<ResultModel> Register(OfficerModel model);

        Task<OfficerModel> GetDetail(string id = null);

        Task<ResultModel> Edit(string id, OfficerModel model);

        Task<PagedResultModel<OfficerLookup>> GetAll(PagedRequestModel model);

        Task<ResultModel> Delete(string id);
    }
}
