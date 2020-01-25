using Application.AppUsers.Commands.ConfirmEmail;
using Application.AppUsers.Commands.Login;
using Application.AppUsers.Commands.RefreshToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshTokenResponse>> Refresh([FromBody]RefreshTokenCommand request)
        {
            var response = await Mediator.Send(request);
            return response;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public string Get()
        {
            return "IN";
        }
    }
}
