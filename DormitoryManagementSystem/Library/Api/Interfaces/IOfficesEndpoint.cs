using System.Threading.Tasks;
using Library.Models;
using Library.Models.Offices;

namespace Library.Api.Interfaces
{
    public interface IOfficesEndpoint
    {
        Task<PagedResultModel<OfficeLookup>> GetAll(PagedRequestModel model);

        Task<OfficeModel> GetDetail(int id);
    }
}
