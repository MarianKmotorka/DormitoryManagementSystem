using System.Threading.Tasks;
using Application.Common.Pagination;
using Application.Offices.Queries.GetOfficeDetail;
using Application.Offices.Queries.GetOfficeList;
using Infrastracture.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace WebApi.Controllers
{
    public class OfficesController : BaseController
    {
        [HttpGet("{id}")]
        [Authorize(PolicyNames.Officer)]
        public async Task<ActionResult<OfficeDetail>> GetOfficeDetail(int id)
        {
            var response = await Mediator.Send(new GetOfficeDetailQuery { Id = id });
            return Ok(response);
        }

        [HttpGet]
        [Authorize(PolicyNames.Officer)]
        public async Task<ActionResult<PagedResponse<OfficeLookup>>> GetOfficeList([FromQuery]SieveModel paginationModel)
        {
            var response = await Mediator.Send(new GetOfficeListQuery { PaginationModel = paginationModel });
            return Ok(response);
        }
    }
}
