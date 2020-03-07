using System.Threading.Tasks;
using Library.Models;
using Library.Models.Repairers;

namespace Library.Api.Interfaces
{
    public interface IRepairersEndpoint
    {
        Task<PropertiesResultModel> Register(RepairerModel model);

        Task<PagedResultModel<RepairerLookup>> GetAll(PagedRequestModel model);

        Task<RepairerModel> GetDetail(string id = null);

        Task<PropertiesResultModel> Edit(string id, RepairerModel model);
    }
}
