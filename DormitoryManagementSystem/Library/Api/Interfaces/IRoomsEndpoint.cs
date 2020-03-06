using System.Threading.Tasks;
using Library.Models;
using Library.Models.Rooms;

namespace Library.Api.Interfaces
{
    public interface IRoomsEndpoint
    {
        Task<PagedResultModel<RoomLookup>> GetAll(PagedRequestModel model);
    }
}
