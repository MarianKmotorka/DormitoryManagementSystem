using System.Threading.Tasks;
using Application.Common.Pagination;
using Application.Rooms.Queries.GetRoomDetail;
using Application.Rooms.Queries.GetRoomList;
using Infrastracture.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace WebApi.Controllers
{
    public class RoomsController : BaseController
    {
        [HttpGet]
        [Authorize(Policy = PolicyNames.Officer)]
        public async Task<ActionResult<PagedResponse<RoomLookup>>> GetRoomList([FromQuery]SieveModel paginationModle)
        {
            var response = await Mediator.Send(new GetRoomListQuery { PaginationModel = paginationModle });
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.Officer)]
        public async Task<ActionResult<RoomDetail>> GetRoomDetail([FromRoute]int id)
        {
            var response = await Mediator.Send(new GetRoomDetailQuery { Id = id });
            return response;
        }
    }
}
