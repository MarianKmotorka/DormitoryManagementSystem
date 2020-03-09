using System.Threading.Tasks;
using Library.Models;
using Library.Models.RepairRequests;

namespace Library.Api.Interfaces
{
    public interface IRepairRequestsEndpoint
    {
        Task<PropertiesResultModel> Create(NewRepairRequestModel model);

        Task<PagedResultModel<RepairRequestLookup>> GetAll(PagedRequestModel model);
    }
}
