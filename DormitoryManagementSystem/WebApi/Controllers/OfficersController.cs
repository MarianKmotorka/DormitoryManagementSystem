using System.Threading.Tasks;
using Application.Common.Pagination;
using Application.Officers.Commands.CreateOfficer;
using Application.Officers.Commands.DeleteOfficer;
using Application.Officers.Queries.GetOfficerDetail;
using Application.Officers.Queries.GetOfficerList;
using Infrastracture.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace WebApi.Controllers
{
    public class OfficersController : BaseController
    {
        [HttpPost]
        [Authorize(PolicyNames.Admin)]
        public async Task<ActionResult> CreateOfficer([FromBody]CreateOfficerCommand request)
        {
            await Mediator.Send(request);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(PolicyNames.Admin)]
        public async Task<ActionResult<OfficerDetail>> GetOfficerDetail(string id)
        {
            var response = await Mediator.Send(new GetOfficerDetailQuery { Id = id });
            return response;
        }

        [HttpGet("me")]
        [Authorize(PolicyNames.Officer)]
        public async Task<ActionResult<OfficerDetail>> GetCurrentOfficerUserDetail()
        {
            var response = await Mediator.Send(new GetOfficerDetailQuery { Id = CurrentUserService.UserId });
            return response;
        }

        [HttpGet]
        [Authorize(PolicyNames.Admin)]
        public async Task<ActionResult<PagedResponse<OfficerLookup>>> GetOfficerList([FromQuery]SieveModel paginationModel)
        {
            var response = await Mediator.Send(new GetOfficerListQuery { PaginationModel = paginationModel });
            return response;
        }

        [HttpDelete("{id}")]
        [Authorize(PolicyNames.Admin)]
        public async Task<ActionResult> DeleteOfficer(string id)
        {
            await Mediator.Send(new DeleteOfficerCommand { Id = id });
            return NoContent();
        }
    }
}
