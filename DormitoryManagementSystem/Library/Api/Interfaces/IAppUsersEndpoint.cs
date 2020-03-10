using System.Threading.Tasks;
using Library.Models;
using Library.Models.Identity;

namespace Library.Api.Interfaces
{
    public interface IAppUsersEndpoint
    {
        Task<ResultModel> Authenticate(string email, string password);

        Task<ResultModel> ConfirmEmail(string email);

        Task<ResultModel> ResetPassword(string email);

        Task<ResultModel> ChangePassword(ChangePasswordModel model);
    }
}
