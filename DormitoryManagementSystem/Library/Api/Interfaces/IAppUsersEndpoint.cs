using System.Threading.Tasks;
using Library.Models;
using Library.Models.Identity;
using Library.Models.Users;

namespace Library.Api.Interfaces
{
    public interface IAppUsersEndpoint
    {
        Task<ResultModel> Authenticate(string email, string password);

        Task<ResultModel> ConfirmEmail(string email);

        Task<ResultModel> ResetPassword(string email);

        Task<ResultModel> ChangePassword(ChangePasswordModel model);

        Task<PagedResultModel<UserLookup>> GetAll(PagedRequestModel model);

        Task<ResultModel> ChangePasswordByAdmin(string id, string newPassword);
    }
}
