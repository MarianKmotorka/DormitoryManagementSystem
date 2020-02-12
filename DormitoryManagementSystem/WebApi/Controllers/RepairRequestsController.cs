using System.Threading.Tasks;
using Application.Common.Pagination;
using Application.RepairRequests.Commands.CreateRepairRequest;
using Application.RepairRequests.Commands.RespondToRepairRequest;
using Application.RepairRequests.Queries.GetRepairRequestDetail;
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
        [Authorize(PolicyNames.Guest)]
        public async Task<ActionResult> CreateRepairRequest([FromBody]CreateRepairRequestCommand request)
        {
            if (request != null) request.GuestId = CurrentUserService.UserId;
            await Mediator.Send(request);
            return NoContent();
        }

        [HttpGet]
        [Authorize(PolicyNames.Repairer)]
        public async Task<ActionResult<PagedResponse<RepairRequestLookup>>> GetRepairRequestList([FromQuery]SieveModel paginationModel)
        {
            var response = await Mediator.Send(new GetRepairRequestListQuery { PaginationModel = paginationModel });
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(PolicyNames.Repairer)]
        public async Task<ActionResult<RepairRequestDetail>> GetRepairRequestDetail(int id)
        {
            var response = await Mediator.Send(new GetRepairRequestDetailQuery { Id = id });
            return response;
        }

        [HttpPatch("{id}")]
        [Authorize(PolicyNames.Repairer)]
        public async Task<ActionResult> RespondToRepairRequest(int id, [FromBody]RespondToRepairRequestCommand request)
        {
            if (request != null)
            {
                request.Id = id;
                request.FixedById = CurrentUserService.UserId;
            }

            await Mediator.Send(request);
            return NoContent();
        }
    }
}
