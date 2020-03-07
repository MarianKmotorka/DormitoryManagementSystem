using System.Threading.Tasks;
using Library.Models;
using Library.Models.Identity;

namespace Library.Api.Interfaces
{
    public interface IAppUsersEndpoint
    {
        Task<ResultModel> Authenticate(string email, string password);

        Task<PropertiesResultModel> ConfirmEmail(string email);

        Task<PropertiesResultModel> ResetPassword(string email);

        Task<PropertiesResultModel> ChangePassword(ChangePasswordModel model);
    }
}
