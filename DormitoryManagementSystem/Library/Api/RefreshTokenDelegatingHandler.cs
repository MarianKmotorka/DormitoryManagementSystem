using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Library.Api.Endpoints;
using Library.Models;
using Newtonsoft.Json.Linq;

namespace Library.Api
{
    public class RefreshTokenDelegatingHandler : DelegatingHandler
    {
        private readonly CurrentUser _currentUser;

        public RefreshTokenDelegatingHandler(CurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Authorization", $"Bearer {_currentUser?.Jwt}");

            var response = await base.SendAsync(request, cancellationToken);

            return response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => await HandleUnauthorized(request),
                _ => response
            };
        }

        private async Task<HttpResponseMessage> HandleUnauthorized(HttpRequestMessage request)
        {
            var response = await base.SendAsync(AppUsersEndpoint.GetRefreshTokenHttpRequestMessage(_currentUser), default);

            var jObject = await response.Content.ReadAsAsync<JObject>();

            _currentUser.Jwt = jObject.Value<string>("jwt");
            _currentUser.RefreshToken = jObject.Value<string>("refreshToken");

            request.Headers.Remove("Authorization");
            request.Headers.Add("Authorization", $"Bearer {_currentUser.Jwt}");

            return await base.SendAsync(request, default);
        }
    }
}
