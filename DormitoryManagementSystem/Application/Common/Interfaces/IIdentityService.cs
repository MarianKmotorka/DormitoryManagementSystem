using Application.Common.Enums;
using Application.Common.Models;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<(Result, string createdUserId)> RegisterUserAsync(string email, string password, AppRoleNames role);
        Task<string> GenerateEmailConfirmationTokenAsync(string email);
        Task<bool> ConfirmEmailAsync(string email, string token);
    }
}
