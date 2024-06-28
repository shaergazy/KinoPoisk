using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using API.Infrastructure;
using Common.Exceptions;
using Common.Extensions;
using DAL.Entities.Users;
using DAL.Extensions;
using DAL.Json;
using DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Services;

/// <summary>
/// 
/// </summary>
public sealed class AuthService
{
    #region Properties
    private UserManager<User> UserManager { get; }

    private AuthDto.Jwt Jwt { get; }
    #endregion

    public AuthService(UserManager<User> userManager, IOptions<AuthDto.Jwt> jwt)
    {
        UserManager = userManager;
        Jwt = jwt.Value;
    }

    public async Task<AuthDto.Response> AccessToken(AuthDto.Login dto)
    {
        var (user, claims, roleNames) = await UserClaimsRoleNames(dto.UserName, dto.Password);
        return BuildResponse(user, claims, roleNames);
    }
    
    private async Task<(User User, IEnumerable<Claim> Claims, IList<string> RoleNames)> UserClaimsRoleNames(string username, string password)
    {
        var user = await UserManager.Users.ByName(username);
        if (!await UserManager.CheckPasswordAsync(user, password))
            throw new InnerException($"2506. Пароль не верный.", nameof(password));

        var (claims, roleNames) = await ClaimsAndRoleNames(user);

        return (user, claims, roleNames);
    }

    private async Task<(IEnumerable<Claim> Claims, IList<string> RoleNames)> ClaimsAndRoleNames(User user)
    {
        var roles = await UserManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id),
        };
        claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

        return (claims, roles);
    }

    private AuthDto.Response BuildResponse(User user, IEnumerable<Claim> claims, IEnumerable<string> roleNames)
    {
        var (accessToken, accessExpireDate) = AccessToken(claims);
        var now = DateTime.UtcNow;
        var refreshExpireDate = now + TimeSpan.FromMinutes(Jwt.RefreshTokenLifeTimeInMinutes);
        var refreshToken = new RefreshToken
        {
            CreateDate = now,
            ExpireDate = refreshExpireDate,
            UserName = user.UserName,
        }.ToJson().DesEncrypt();

        return new AuthDto.Response
        {
            AccessToken = accessToken,
            AccessTokenExpireDate = accessExpireDate,
            RefreshToken = refreshToken,
            RefreshTokenExpireDate = refreshExpireDate,
            UserName = user.UserName,
            RoleNames = roleNames,
        };
    }

    private (string AccessToken, DateTime Expires) AccessToken(IEnumerable<Claim> claims)
    {
        var (token, expires) = JwtBuilder.SecurityToken(claims, Jwt);
        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }

    public async Task<AuthDto.Response> RefreshToken(AuthDto.Refresh dto)
    {
        if (!dto.RefreshToken.IsBase64Str())
            throw new InnerException($"2514. Невалидный токен.", nameof(dto.RefreshToken));

        var refreshToken = dto.RefreshToken.DesDecrypt().FromJson<RefreshToken>();
        if (refreshToken.ExpireDate <= DateTime.UtcNow)
            throw new InnerException($"2503. Срок действия refresh токена истёк.");

        var user = await UserManager.Users.ByName(refreshToken.UserName);
        var (claims, roleNames) = await ClaimsAndRoleNames(user);

        return BuildResponse(user, claims, roleNames);
    }
}
