using Application.Common.Interfaces;
using Infrastracture.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infrastracture.Messaging
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceOptions _emailServiceOptions;
        private readonly ILogger<EmailService> _logger;

        public EmailService(EmailServiceOptions emailServiceOptions, ILogger<EmailService> logger)
        {
            _emailServiceOptions = emailServiceOptions;
            _logger = logger;
        }

        public async Task SendAsync(string message, string receiverEmail, string subject, bool isMessageHtml = true)
        {
            //https://myaccount.google.com/lesssecureapps

            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                mailMessage.From = new MailAddress("noreply@tweetbook.com");
                mailMessage.To.Add(new MailAddress(receiverEmail));
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = isMessageHtml;
                mailMessage.Body = message;
                smtp.Port = _emailServiceOptions.Port;
                smtp.Host = _emailServiceOptions.Host;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_emailServiceOptions.Email, _emailServiceOptions.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"MESSAGE TO {receiverEmail} WAS NOT SENT. REASON: {ex.Message}");
            }
        }
    }
}
