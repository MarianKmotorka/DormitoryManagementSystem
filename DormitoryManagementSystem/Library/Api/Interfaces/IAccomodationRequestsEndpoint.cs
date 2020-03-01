using System.Threading.Tasks;
using Library.Models;
using Library.Models.AccomodationRequests;

namespace Library.Api.Interfaces
{
    public interface IAccomodationRequestsEndpoint
    {
        Task<PagedResultModel<AccomodationRequestLookup>> GetAll(PagedRequestModel model);
    }
}
