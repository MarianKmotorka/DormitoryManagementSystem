using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastracture.Identity
{
    public class IdentityService : IIdentityService
    {
        private UserManager<AppUser> _userManager;

        public IdentityService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(Result, string createdUserId)> RegisterUserAsync(string email, string password, AppRoleNames role)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
                return (Result.Failure(new[] { "Email is not unique" }), null);

            var newUser = new AppUser
            {
                Email = email,
                UserName = email.Split('@')[0],
                Address = Address.Empty
            };

            var identityResult = await _userManager.CreateAsync(newUser, password);

            if (!identityResult.Succeeded)
                return (Result.Failure(identityResult.Errors.Select(x => x.Description)), null);

            identityResult = await _userManager.AddToRoleAsync(newUser, role.ToString());

            return (identityResult.ToApplicationResult(), newUser.Id);
        }
    }
}
