using System.Threading.Tasks;
using Library.Models;
using Library.Models.Guests;

namespace Library.Api.Interfaces
{
    public interface IGuestsEndpoint
    {
        Task<PropertiesResultModel> RegisterGuest(GuestModel model);

        Task<GuestModel> GetGuestDetail(string id = null);

        Task<PropertiesResultModel> EditGuest(string id, GuestModel model);
    }
}
