using System.Threading.Tasks;
using Library.Models;
using Library.Models.RepairRequests;

namespace Library.Api.Interfaces
{
    public interface IRepairRequestsEndpoint
    {
        Task<ResultModel> Create(NewRepairRequestModel model);

        Task<PagedResultModel<RepairRequestLookup>> GetAll(PagedRequestModel model);

        Task<RepairRequestModel> GetDetail(int id);

        Task<ResultModel> RespondToRepairRequest(int id, RespondToRepairRequestModel model);
    }
}
