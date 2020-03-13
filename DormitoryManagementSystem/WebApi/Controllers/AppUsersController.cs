using System.Threading.Tasks;
using Application.AppUsers.Commands.ChangeForgottenPassword;
using Application.AppUsers.Commands.ChangePassword;
using Application.AppUsers.Commands.ChangePasswordByAdmin;
using Application.AppUsers.Commands.ConfirmEmail;
using Application.AppUsers.Commands.Login;
using Application.AppUsers.Commands.RefreshToken;
using Application.AppUsers.Commands.SendChangeForgottenPasswordEmail;
using Application.AppUsers.Commands.SendConfirmationEmail;
using Application.AppUsers.Queries.GetAppUserList;
using Application.Common.Pagination;
using Infrastracture.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace WebApi.Controllers
{
    public class AppUsersController : BaseController
    {

        [HttpGet("confirmation-email")]
        public async Task<ActionResult> SendConfirmationEmail([FromQuery]string email = "")
        {
            await Mediator.Send(new SendConfirmationEmailCommand { Email = email });
            return Ok();
        }

        [HttpPost("confirm")]
        public async Task<ActionResult<string>> ConfirmEmail([FromForm]ConfirmEmailCommand request)
        {
            await Mediator.Send(request);
            return "Email was successfully confirmed";
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody]LoginCommand request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshTokenResponse>> Refresh([FromBody]RefreshTokenCommand request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody]ChangePasswordCommand request)
        {
            if (request != null) request.UserId = CurrentUserService.UserId;
            await Mediator.Send(request);
            return NoContent();
        }

        [HttpPost("forgotten-password")]
        public async Task<ActionResult<string>> ChangeForgottenPassword([FromForm]ChangeForgottenPasswordCommand request)
        {
            await Mediator.Send(request);
            return "Password was successfully changed";
        }

        [HttpGet("forgotten-password")]
        public async Task<ActionResult> SendChangeForgottenPasswordEmail([FromQuery]string email = "")
        {
            await Mediator.Send(new SendChangeForgottenPasswordEmailCommand { Email = email });
            return Ok();
        }

        [HttpGet]
        [Authorize(PolicyNames.Admin)]
        public async Task<ActionResult<PagedResponse<AppUserLookup>>> Get([FromQuery]SieveModel paginationModel)
        {
            var response = await Mediator.Send(new GetAppUserListQuery { PaginationModel = paginationModel });
            return Ok(response);
        }

        [HttpPost("{id}/password")]
        [Authorize(PolicyNames.Admin)]
        public async Task<ActionResult> ChangePasswordByAdmin(string id, ChangePasswordByAdminCommand request)
        {
            if (request != null) request.Id = id;
            await Mediator.Send(request);
            return NoContent();
        }
    }
}
