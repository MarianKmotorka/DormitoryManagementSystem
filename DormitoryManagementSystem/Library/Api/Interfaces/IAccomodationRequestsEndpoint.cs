using System.Threading.Tasks;
using Library.Models;
using Library.Models.AccomodationRequests;

namespace Library.Api.Interfaces
{
    public interface IAccomodationRequestsEndpoint
    {
        Task<PagedResultModel<AccomodationRequestLookup>> GetAll(PagedRequestModel model);

        Task<AccomodationRequestDetail> GetDetail(int id);

        Task<ResultModel> RejectAccomodationRequest(int requestId, string additionalMessage);

        Task<ResultModel> ApproveAccomodationRequest(int requestId, int roomId, string additionalMessage);

        Task<ResultModel> Create(NewAccomodationRequestModel model);
    }
}
