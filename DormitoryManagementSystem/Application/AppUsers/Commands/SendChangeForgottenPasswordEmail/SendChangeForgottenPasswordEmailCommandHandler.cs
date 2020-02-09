using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.AppUsers.Commands.SendChangeForgottenPasswordEmail
{
    public class SendChangeForgottenPasswordEmailCommandHandler : IRequestHandler<SendChangeForgottenPasswordEmailCommand>
    {
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendChangeForgottenPasswordEmailCommandHandler(IEmailService emailService, IIdentityService identityService, IHttpContextAccessor httpContextAccessor)
        {
            _emailService = emailService;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(SendChangeForgottenPasswordEmailCommand request, CancellationToken cancellationToken)
        {
            var token = await _identityService.GenerateChangeForgottenPasswordTokenAsync(request.Email);
            var endpoint = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/{request.ChangeForgottenPasswordRoute}";

            var message = $@"<!DOCTYPE html>
                             <html>
                             <head>
                             <style>
                                 .button {{
                                     background-color: #4CAF50;
                                     width: 75%;
                                     border: none;
                                     color: white;
                                     padding: 15px 32px;
                                     text-align: center;
                                     text-decoration: none;
                                     display: inline-block;
                                     font-size: 16px;
                                     cursor: pointer;
                                     border-radius: 4px;
                                    }}
                                 .password {{
                                     width: 75%;
                                     padding: 12px 20px;
                                     margin: 8px 0;
                                     display: inline-block;
                                     border: 1px solid #ccc;
                                     border-radius: 4px;
                                     box-sizing: border-box;
                                   }}
                             </style>
                             </head>
                             <body>
                                    
                             <h3>Please fill in your new password. Password must contain at least 6 characters.</h3>

                             <form method=""post"" action=""{endpoint}"">

                                 <input type=""text"" name=""newPassword"" class=""password"" />
                                 <input type=""hidden"" name=""email"" value=""{request.Email}"" />
                                 <button type=""submit"" name=""resetToken"" value=""{token}"" class=""button"">
                                    Change Password
                                 </button>

                             </form>

                             </body>
                             </html>
                             ";


            await _emailService.SendAsync(message, request.Email, "Change forgotten password");

            return Unit.Value;
        }
    }
}
