using System.Threading.Tasks;
using Library.Models;

namespace Library.Api.Interfaces
{
    public interface IGuestsEndpoint
    {
        Task<PropertiesResultModel> RegisterGuest(GuestRegistrationModel model);
    }
}
