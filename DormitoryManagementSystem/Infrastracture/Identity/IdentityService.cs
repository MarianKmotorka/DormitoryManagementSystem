﻿using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastracture.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastracture.Identity
{
    public class IdentityService : IIdentityService
    {
        private UserManager<AppUser> _userManager;
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

        public async Task<(Result, string createdUserId)> RegisterUserAsync(string email, string password, AppRoleNames role)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
                return (Result.Failure(ErrorMessages.EmailNotUnique), null);

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
    }
}
