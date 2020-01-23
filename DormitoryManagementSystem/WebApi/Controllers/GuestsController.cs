using Application.Guests.Commands.CreateGuest;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
    }
}
