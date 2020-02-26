using System.Threading.Tasks;
using Library.Models;
using Library.Models.Guests;

namespace Library.Api.Interfaces
{
    public interface IGuestsEndpoint
    {
        Task<PropertiesResultModel> Register(GuestModel model);

        Task<GuestModel> GetDetail(string id = null);

        Task<PropertiesResultModel> Edit(string id, GuestModel model);
    }
}
