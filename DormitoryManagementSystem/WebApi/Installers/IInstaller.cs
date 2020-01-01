using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Installers
{
    public interface IInstaller
    {
        IServiceCollection Install(IServiceCollection services, IConfiguration configuration);
    }
}
