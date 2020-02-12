using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastracture.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastracture.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IDormitoryDbContext _db;
        private readonly JwtOptions _jwtOptions;

        public IdentityService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            JwtOptions jwtOptions, TokenValidationParameters tokenValidationParameters,
            IDormitoryDbContext db)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenValidationParameters = tokenValidationParameters;
            _jwtOptions = jwtOptions;
        }

        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            var appUser = await _userManager.FindByEmailAsync(email);

            if (appUser is null) return false;

            var result = await _userManager.ConfirmEmailAsync(appUser, token);

            return result.Succeeded;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            return await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
        }

        public async Task<(Result, string jwt, string refreshToken)> LoginUserAsync(string email, string password)
        {
            var appUser = await _userManager.FindByEmailAsync(email);

            if (appUser == null)
                return (Result.Failure(ErrorMessages.InvalidEmailOrPassword), null, null);

            if (!await _userManager.CheckPasswordAsync(appUser, password))
                return (Result.Failure(ErrorMessages.InvalidEmailOrPassword), null, null);

            if (!await _userManager.IsEmailConfirmedAsync(appUser))
                return (Result.Failure(ErrorMessages.EmailNotConfirmed), null, null);

            return await GenerateJwtAndRefreshToken(appUser);
        }

        public async Task<(Result, string createdUserId)> RegisterUserAsync(string firstName, string lastName, string email, string password, AppRoleNames role)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
                return (Result.Failure(ErrorMessages.EmailNotUnique), null);

            var newUser = new AppUser
            {
                Email = email,
                UserName = email,
                Address = Address.Empty,
                FirstName = firstName,
                LastName = lastName
            };

            var identityResult = await _userManager.CreateAsync(newUser, password);

            if (!identityResult.Succeeded)
                return (Result.Failure(identityResult.Errors.Select(x => x.Description)), null);

            identityResult = await _userManager.AddToRoleAsync(newUser, role.ToString());

            return (identityResult.ToApplicationResult(), newUser.Id);
        }

        public async Task<(Result, string jwt, string refreshToken)> RefreshJwtAsync(string expiredJwt, string refreshToken)
        {
            var validatedJwt = GetPrincipalFromJwt(expiredJwt);

            if (validatedJwt == null) return (Result.Failure(ErrorMessages.Invalid), null, null);

            var expiryDateUnix =
                long.Parse(validatedJwt.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateUtc =
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);

            if (expiryDateUtc > DateTime.UtcNow) return (Result.Failure(ErrorMessages.JwtIsNotExpired), null, null);

            var jwtId = validatedJwt.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _db.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if (storedRefreshToken == null) return (Result.Failure(ErrorMessages.RefreshTokenDoesNotExist), null, null);

            if (storedRefreshToken.ExpiryDate < DateTime.UtcNow) return (Result.Failure(ErrorMessages.RefreshTokenExpired), null, null);

            if (storedRefreshToken.Used) return (Result.Failure(ErrorMessages.RefreshTokenAlreadyUsed), null, null);

            if (storedRefreshToken.JwtId != jwtId) return (Result.Failure(ErrorMessages.RefreshTokenDoesNotMatchJwt), null, null);

            storedRefreshToken.Used = true;
            await _db.SaveChangesAsync(CancellationToken.None);

            var appUserId = validatedJwt.Claims.Single(c => c.Type == "appUserId").Value;
            var appUser = await _db.Users.SingleAsync(x => x.Id == appUserId);

            return await GenerateJwtAndRefreshToken(appUser);
        }

        private ClaimsPrincipal GetPrincipalFromJwt(string jwt)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = _tokenValidationParameters.Clone();
            tokenValidationParameters.ValidateLifetime = false;

            try
            {
                var principal = jwtHandler.ValidateToken(jwt, tokenValidationParameters, out var validatedJwt);

                var hasJwtValidSecurityAlgorithm =
                    (validatedJwt is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                if (!hasJwtValidSecurityAlgorithm) return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private async Task<(Result, string jwt, string refreshToken)> GenerateJwtAndRefreshToken(AppUser appUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, appUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim("appUserId", appUser.Id)
            };

            var userClaims = await _userManager.GetClaimsAsync(appUser);
            claims.AddRange(userClaims);

            var roleClaims = await GetRoleClaims(appUser);
            claims.AddRange(roleClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtOptions.TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                AppUserId = appUser.Id,
                JwtId = token.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            _db.RefreshTokens.Add(refreshToken);
            await _db.SaveChangesAsync(CancellationToken.None);

            return (Result.Success(), tokenHandler.WriteToken(token), refreshToken.Token);
        }

        private async Task<List<Claim>> GetRoleClaims(AppUser appUser)
        {
            var claims = new List<Claim>();

            var userRoles = await _userManager.GetRolesAsync(appUser);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                        continue;

                    claims.Add(roleClaim);
                }
            }

            return claims;
        }

        public async Task<string> GenerateChangeForgottenPasswordTokenAsync(string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            return await _userManager.GeneratePasswordResetTokenAsync(appUser);
        }

        public async Task<Result> ChangePassword(string email, string currentPassword, string newPassword)
        {
            var appUser = await _userManager.FindByEmailAsync(email);

            if (appUser == null)
                return Result.Failure(ErrorMessages.EmailNotFound);

            return (await _userManager.ChangePasswordAsync(appUser, currentPassword, newPassword)).ToApplicationResult();
        }

        public async Task<Result> ChangeForgottenPasswordAsync(string email, string resetToken, string newPassword)
        {
            var appUser = await _userManager.FindByEmailAsync(email);

            if (appUser == null)
                return Result.Failure(ErrorMessages.EmailNotFound);

            return (await _userManager.ResetPasswordAsync(appUser, resetToken, newPassword)).ToApplicationResult();
        }

        public async Task<(Result, string)> GetRole(string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email);

            if (appUser == null)
                return (Result.Failure(ErrorMessages.EmailNotFound), null);

            var role = (await _userManager.GetRolesAsync(appUser)).Single();

            return (Result.Success(), role);
        }
    }
}
