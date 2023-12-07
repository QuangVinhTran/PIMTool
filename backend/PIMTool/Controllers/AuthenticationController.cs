using Microsoft.AspNetCore.Mvc;
using PIMTool.Controllers.Base;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Core.Models;

namespace PIMTool.Controllers;

[Route("auth")]
public class AuthenticationController : BaseController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IJwtService _jwtService;

    public AuthenticationController(IAuthenticationService authenticationService, IJwtService jwtService)
    {
        _authenticationService = authenticationService;
        _jwtService = jwtService;
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(UserLoginModel userLogin)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "";
        
        return await ExecuteApiAsync(
            async () => await _authenticationService.LoginAsync(userLogin, ipAddress).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterModel userRegister)
    {
        return await ExecuteApiAsync(
            async () => await _authenticationService.RegisterAsync(userRegister).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [Route("refresh/{refreshToken}")]
    [HttpPost]
    public async Task<IActionResult> RefreshToken(string refreshToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "";
        
        return await ExecuteApiAsync(
            async () => await _jwtService.RefreshToken(refreshToken, ipAddress).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
}