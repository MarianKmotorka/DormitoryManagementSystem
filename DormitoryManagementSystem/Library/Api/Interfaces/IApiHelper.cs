using System.Net.Http;

namespace Library.Api.Interfaces
{
    public interface IApiHelper
    {
        HttpClient Client { get; }

        void LogOut();
    }
}
