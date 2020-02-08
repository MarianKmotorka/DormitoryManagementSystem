using System.Threading.Tasks;
using Application.AccomodationRequests.Queries.GetAccomodationRequestList;
using Application.Common.Pagination;
using Application.Guests.Commands.CreateGuest;
using Application.Guests.Queries.GetGuestDetail;
using Application.Guests.Queries.GetGuestList;
using Application.Guests.Queries.GetMyAccomodationRequestList;
using Infrastracture.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace WebApi.Controllers
{
    public class GuestsController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult> CreateGuest([FromBody] CreateGuestCommand request)
        {
            await Mediator.Send(request);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.Officer)]
        public async Task<ActionResult<PagedResponse<GuestLookup>>> GetGuestList([FromQuery]SieveModel paginationModel)
        {
            var response = await Mediator.Send(new GetGuestListQuery { PaginationModel = paginationModel });
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.Officer)]
        public async Task<ActionResult<GuestDetail>> GetGuestDetail([FromRoute]string id)
        {
            var response = await Mediator.Send(new GetGuestDetailQuery { Id = id });
            return response;
        }

        [HttpGet("me")]
        [Authorize(Policy = PolicyNames.Guest)]
        public async Task<ActionResult<GuestDetail>> GetCurrentGuestUserDetail()
        {
            var response = await Mediator.Send(new GetGuestDetailQuery { Id = CurrentUserService.UserId });
            return response;
        }

        [HttpGet("me/accomodation-requests")]
        [Authorize(Policy = PolicyNames.Guest)]
        public async Task<ActionResult<PagedResponse<AccomodationRequestLookup>>> GetMyAccomodationRequestList([FromQuery]SieveModel paginationModel)
        {
            var response = await Mediator.Send(new GetMyAccomodationRequestListQuery { PaginationModel = paginationModel, GuestId = CurrentUserService.UserId });
            return response;
        }
    }
}
