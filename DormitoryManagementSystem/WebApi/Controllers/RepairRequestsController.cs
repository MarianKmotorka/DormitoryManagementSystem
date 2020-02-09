using System.Threading.Tasks;
using Application.Common.Pagination;
using Application.RepairRequests.Commands.CreateRepairRequest;
using Application.RepairRequests.Queries.GetRepairRequestList;
using Infrastracture.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace WebApi.Controllers
{
    public class RepairRequestsController : BaseController
    {
        [HttpPost]
        [Authorize(Policy = PolicyNames.Guest)]
        public async Task<ActionResult> CreateRepairRequest([FromBody]CreateRepairRequestCommand request)
        {
            request.GuestId = CurrentUserService.UserId;
            await Mediator.Send(request);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.Repairer)]
        public async Task<ActionResult<PagedResponse<RepairRequestLookup>>> GetRepairRequestList([FromQuery]SieveModel paginationModel)
        {
            var response = await Mediator.Send(new GetRepairRequestListQuery { PaginationModel = paginationModel });
            return response;
        }
    }
}
