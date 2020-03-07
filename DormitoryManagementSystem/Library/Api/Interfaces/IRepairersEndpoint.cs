using System.Threading.Tasks;
using Library.Models;
using Library.Models.Repairers;

namespace Library.Api.Interfaces
{
    public interface IRepairersEndpoint
    {
        Task<PropertiesResultModel> Register(RepairerModel model);
    }
}
