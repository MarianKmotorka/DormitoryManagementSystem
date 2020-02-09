using System.Threading.Tasks;
using Application.Repairers.Commands.CreateRepairer;
using Infrastracture.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class RepairersController : BaseController
    {
        [HttpPost]
        [Authorize(Policy = PolicyNames.Officer)]
        public async Task<ActionResult> CreateRepairer([FromBody]CreateRepairerCommand request)
        {
            await Mediator.Send(request);
            return NoContent();
        }
    }
}
