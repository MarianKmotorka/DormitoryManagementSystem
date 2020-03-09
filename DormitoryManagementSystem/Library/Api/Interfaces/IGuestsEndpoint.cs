using System.Threading.Tasks;
using Library.Models;
using Library.Models.AccomodationRequests;
using Library.Models.Guests;
using Library.Models.RepairRequests;
using Library.Models.Rooms;

namespace Library.Api.Interfaces
{
    public interface IGuestsEndpoint
    {
        Task<PropertiesResultModel> Register(GuestModel model);

        Task<GuestModel> GetDetail(string id = null);

        Task<PagedResultModel<GuestLookup>> GetAll(PagedRequestModel model);

        Task<PropertiesResultModel> Edit(string id, GuestModel model);

        Task<PagedResultModel<AccomodationRequestLookup>> GetMyAccomodationRequests(PagedRequestModel model);

        Task<RoomModel> GetMyRoomDetail();

        Task<PagedResultModel<RepairRequestLookup>> GetMyRepairRequests(PagedRequestModel pagedRequestModel);

        Task<RepairRequestModel> GetMyRepairRequestDetail(int requestId);
    }
}
