using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string message, string receiverEmail, string subject, bool isMessageHtml = true);
    }
}
