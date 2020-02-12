using System.Threading.Tasks;
using Application.AccomodationRequests.Commands.CreateAccomodationRequest;
using Application.AccomodationRequests.Commands.RespondToAccomodationRequest;
using Application.AccomodationRequests.Queries.GetAccomodationRequestDetail;
using Application.AccomodationRequests.Queries.GetAccomodationRequestList;
using Application.Common.Pagination;
using Infrastracture.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace WebApi.Controllers
{
    public class AccomodationRequestsController : BaseController
    {
        [HttpPost]
        [Authorize(Policy = PolicyNames.Guest)]
        public async Task<ActionResult> CreateAccomodationRequest([FromBody]CreateAccomodationRequestCommand request)
        {
            if (request != null) request.RequesterId = CurrentUserService.UserId;
            await Mediator.Send(request);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.Officer)]
        public async Task<ActionResult<PagedResponse<AccomodationRequestLookup>>> GetAccomodationRequestList([FromQuery]SieveModel paginationModel)
        {
            var response = await Mediator.Send(new GetAccomodationRequestListQuery { PaginationModel = paginationModel });
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.Officer)]
        public async Task<ActionResult<AccomodationRequestDetail>> GetAccomodationRequestDetail(int id)
        {
            var response = await Mediator.Send(new GetAccomodationRequestDetailQuery { Id = id });
            return response;
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = PolicyNames.Officer)]
        public async Task<ActionResult> RespondToAccomodationRequest(int id, [FromBody]RespondToAccomodationRequestCommand request)
        {
            if (request != null) request.AccomodationRequestId = id;
            await Mediator.Send(request);
            return NoContent();
        }
    }
}
