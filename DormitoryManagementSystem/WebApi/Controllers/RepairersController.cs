using System.Threading.Tasks;
using Application.Common.Pagination;
using Application.Repairers.Commands.CreateRepairer;
using Application.Repairers.Commands.DeleteRepairer;
using Application.Repairers.Queries.GetRepairerDetail;
using Application.Repairers.Queries.GetRepairerList;
using Infrastracture.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace WebApi.Controllers
{
    public class RepairersController : BaseController
    {
        [HttpPost]
        [Authorize(PolicyNames.Officer)]
        public async Task<ActionResult> CreateRepairer([FromBody]CreateRepairerCommand request)
        {
            await Mediator.Send(request);
            return NoContent();
        }

        [HttpGet]
        [Authorize(PolicyNames.Officer)]
        public async Task<ActionResult<PagedResponse<RepairerLookup>>> GetRepairerList([FromQuery]SieveModel paginationModel)
        {
            var response = await Mediator.Send(new GetRepairerListQuery { PaginationModel = paginationModel });
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(PolicyNames.Officer)]
        public async Task<ActionResult<RepairerDetail>> GetRepairerDetail(string id)
        {
            var response = await Mediator.Send(new GetRepairerDetailQuery { Id = id });
            return response;
        }

        [HttpGet("me")]
        [Authorize(PolicyNames.Repairer)]
        public async Task<ActionResult<RepairerDetail>> GetMyRepairerDetail()
        {
            var response = await Mediator.Send(new GetRepairerDetailQuery { Id = CurrentUserService.UserId });
            return response;
        }

        [HttpDelete("{id}")]
        [Authorize(PolicyNames.Officer)]
        public async Task<ActionResult> DeleteRepairer(string id)
        {
            await Mediator.Send(new DeleteRepairerCommand { Id = id });
            return NoContent();
        }
    }
}
