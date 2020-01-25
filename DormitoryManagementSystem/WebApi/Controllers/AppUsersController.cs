using Application.AppUsers.Commands.ConfirmEmail;
using Application.AppUsers.Commands.Login;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class AppUsersController : BaseController
    {
        [HttpGet("confirm")]
        public async Task<ActionResult<string>> ConfirmEmail([FromQuery]string email = "", [FromQuery]string token = "")
        {
            await Mediator.Send(new ConfirmEmailCommand { Email = email, Token = token });
            return "Email was successfully confirmed";
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody]LoginCommand request)
        {
            var response = await Mediator.Send(request);
            return response;
        }
    }
}
