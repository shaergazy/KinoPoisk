using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DTO;

namespace API.Controllers;

/// <summary>
/// Controller for work with authentication
/// </summary>
[Route("api/Auth")]
public class AuthController : ControllerBase
{
    private AuthService Service { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public AuthController(AuthService service)
    {
        Service = service;
    }

    /// <summary>
    /// Get access and refresh tokens by login and password
    /// </summary>
    /// <param name="dto">login and pwd</param>
    /// <response code="400">payload error</response>
    /// <response code="500">uncaught, unknown error</response>
    [HttpPost]
    [Route("Access")]
    [ProducesResponseType(typeof(AuthDto.Response), StatusCodes.Status200OK)]
    public async Task<AuthDto.Response> AccessToken(AuthDto.Login dto)
    {
        return await Service.AccessToken(dto);
    }

    /// <summary>
    /// Get access and refresh tokens by old refresh token
    /// </summary>
    /// <param name="dto">old refresh token</param>
    /// <response code="400">payload error</response>
    /// <response code="500">uncaught, unknown error</response>
    [HttpPost]
    [Route("Refresh")]
    [ProducesResponseType(typeof(AuthDto.Response), StatusCodes.Status200OK)]
    public async Task<AuthDto.Response> RefreshToken(AuthDto.Refresh dto)
    {
        return await Service.RefreshToken(dto);
    }
}