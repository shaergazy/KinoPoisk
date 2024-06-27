using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Infrastructure;

internal static class JwtBuilder
{
    internal static TokenValidationParameters Parameters(IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(AuthDto.Jwt)).Get<AuthDto.Jwt>();
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = settings.Issuer,
            ValidAudience = settings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
            ClockSkew = TimeSpan.Zero,
        };
    }

    internal static (JwtSecurityToken Token, DateTime Expire) SecurityToken(IEnumerable<Claim> claims, AuthDto.Jwt jwt)
    {
        var utcNow = DateTime.UtcNow;
        var expires = utcNow.Add(TimeSpan.FromMinutes(jwt.AccessTokenLifeTimeInMinutes));
        return (new JwtSecurityToken(
            issuer: jwt.Issuer,
            audience: jwt.Audience,
            claims: claims,
            notBefore: utcNow,
            expires: expires,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt.Key)),
                SecurityAlgorithms.HmacSha256Signature)), expires);
    }

    internal static string Bearer()
    {
        return nameof(Bearer).ToLower();
    }

    internal static string Authorization()
    {
        return nameof(Authorization);
    }
}
