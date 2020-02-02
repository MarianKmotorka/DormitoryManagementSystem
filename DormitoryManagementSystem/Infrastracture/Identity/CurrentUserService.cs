using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Infrastracture.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var userClaims = httpContextAccessor.HttpContext.User.Claims;

            UserId = userClaims.Single(x => x.Type == "appUserId").Value;
            Role = userClaims.Single(x => x.Type == ClaimTypes.Role).Value;
        }

        public string UserId { get; }

        public string Role { get; }
    }
}
