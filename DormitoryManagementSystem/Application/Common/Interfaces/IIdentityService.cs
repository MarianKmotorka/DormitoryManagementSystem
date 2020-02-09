using System.Threading.Tasks;
using Application.Common.Enums;
using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<(Result, string createdUserId)> RegisterUserAsync(string firstName, string lastName, string email, string password, AppRoleNames role);
        Task<(Result, string jwt, string refreshToken)> LoginUserAsync(string email, string password);
        Task<(Result, string jwt, string refreshToken)> RefreshJwtAsync(string expiredJwt, string refreshToken);

        Task<string> GenerateEmailConfirmationTokenAsync(string email);
        Task<string> GenerateChangeForgottenPasswordTokenAsync(string email);

        Task<Result> ChangePassword(string email, string currentPassword, string newPassword);
        Task<Result> ChangeForgottenPasswordAsync(string email, string resetToken, string newPassword);

        Task<bool> ConfirmEmailAsync(string email, string token);
    }
}
