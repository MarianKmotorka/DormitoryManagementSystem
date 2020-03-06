using System.Threading.Tasks;
using Library.Models;
using Library.Models.AccomodationRequests;

namespace Library.Api.Interfaces
{
    public interface IAccomodationRequestsEndpoint
    {
        Task<PagedResultModel<AccomodationRequestLookup>> GetAll(PagedRequestModel model);

        Task<AccomodationRequestDetail> GetDetail(int id);

        Task<PropertiesResultModel> RejectAccomodationRequest(int requestId, string additionalMessage);

        Task<PropertiesResultModel> ApproveAccomodationRequest(int requestId, int roomId, string additionalMessage);

        Task<PropertiesResultModel> Create(NewAccomodationRequestModel model);
    }
}
