using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

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

			var message = $@"<!DOCTYPE html>
                             <html>
                             <head>
                             <style>
                                 .button {{
                                     background-color: #4CAF50;
                                     border: none;
                                     color: white;
                                     padding: 15px 32px;
                                     text-align: center;
                                     text-decoration: none;
                                     display: inline-block;
                                     font-size: 16px;
                                     cursor: pointer;
                             }}
                             </style>
                             </head>
                             <body>
                                    
                             <h3>Press the confirmation button to confirm your email address.</h3>

                             <form method=""post"" action=""{confirmationEndpoint}"">

                                 <input type=""hidden"" name=""email"" value=""{request.Email}"">
                                 <button type=""submit"" name=""token"" value=""{token}"" class=""button"">
                                 Confirm
                                 </button>

                             </form>

                             </body>
                             </html>
                             ";

			await _emailService.SendAsync(message, request.Email, "Email confirmation");

			return Unit.Value;
		}
	}
}
