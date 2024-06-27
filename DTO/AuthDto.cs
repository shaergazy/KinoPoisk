using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO;

/// <summary>
/// 
/// </summary>
public static class AuthDto
{
    /// <summary>
    /// 
    /// </summary>
    public sealed record Login
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string UserName { get; init; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Password { get; init; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed record Refresh
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string RefreshToken { get; init; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed record Response
    {
        /// <summary>
        /// 
        /// </summary>
        public string AccessToken { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime AccessTokenExpireDate { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string RefreshToken { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime RefreshTokenExpireDate { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> RoleNames { get; init; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed record Jwt
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string Audience { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public long AccessTokenLifeTimeInMinutes { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public long RefreshTokenLifeTimeInMinutes { get; init; }
    }
}
