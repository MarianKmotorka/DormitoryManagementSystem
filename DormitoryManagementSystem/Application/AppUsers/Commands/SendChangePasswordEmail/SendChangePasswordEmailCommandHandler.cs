using Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.AppUsers.Commands.SendChangePasswordEmail
{
    public class SendChangePasswordEmailCommandHandler : IRequestHandler<SendChangePasswordEmailCommand>
    {
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;

        public SendChangePasswordEmailCommandHandler(IEmailService emailService, IIdentityService identityService)
        {
            _emailService = emailService;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(SendChangePasswordEmailCommand request, CancellationToken cancellationToken)
        {
            var changePasswordToken = await _identityService.GenerateChangePasswordTokenAsync(request.Email);

            var message = $@"<p>{request.AdditionalMessage}/</p>
                            <br/><br/>
                            <p>Use this token to change your password: 
                            <br/><br/>{changePasswordToken}
                            <br/><br/></p>";

            await _emailService.SendAsync(message, request.Email, "Change password token");

            return Unit.Value;
        }
    }
}
