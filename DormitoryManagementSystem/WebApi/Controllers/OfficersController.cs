using Application.Officers.Commands.CreateOfficer;
using Infrastracture.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize(Policy = PolicyNames.Officer)]
    public class OfficersController : BaseController
    {
        [HttpPost]
        [Authorize(Policy = PolicyNames.Admin)]
        public async Task<ActionResult> CreateOfficer([FromBody]CreateOfficerCommand request)
        {
            await Mediator.Send(request);
            return NoContent();
        }
    }
}
