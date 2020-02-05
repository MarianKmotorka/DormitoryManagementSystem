using System.Threading.Tasks;
using Application.AccomodationRequests.Commands.CreateAccomodationRequest;
using Infrastracture.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class AccomodationRequestsController : BaseController
    {
        [HttpPost]
        [Authorize(Policy = PolicyNames.Guest)]
        public async Task<ActionResult> CreateAccomodationRequest([FromBody]CreateAccomodationRequestCommand request)
        {
            request.RequestorId = CurrentUserService.UserId;
            await Mediator.Send(request);
            return NoContent();
        }
    }
}
