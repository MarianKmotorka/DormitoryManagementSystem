using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Application.AppUsers.Commands.SendConfirmationEmail
{
    public class SendConfirmationEmaiCommandHanlder : IRequestHandler<SendConfirmationEmailCommand>
    {
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendConfirmationEmaiCommandHanlder(IEmailService emailService, IIdentityService identityService, IHttpContextAccessor httpContextAccessor)
        {
            _emailService = emailService;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            var token = await _identityService.GenerateEmailConfirmationTokenAsync(request.Email);

            var confirmationEndpoint = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/{request.ConfirmationRoute}";
            var confirmationLink = $"{confirmationEndpoint}?email={request.Email}&token={HttpUtility.UrlEncode(token)}";

            var message = $@"<a href=""{confirmationLink}""><h2>Confirm email</h2></a>";

            await _emailService.SendAsync(message, request.Email, "Email confirmation");

            return Unit.Value;
        }
    }
}
