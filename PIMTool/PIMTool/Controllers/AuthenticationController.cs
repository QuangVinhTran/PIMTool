using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PIMTool.Entities;
using PIMTool.Payload.Request.Authentication;
using PIMTool.Payload.Response;
using PIMTool.Services;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PIMTool.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private IConfiguration _configuration;
    private IAuthenticationService _service;
    private IMapper _mapper;

    private List<UserAuthentication> users = new List<UserAuthentication>()
    {
        new UserAuthentication("nguyenho", "123456"),
        new UserAuthentication("quynhgiang", "123456"),
        new UserAuthentication("quynngfamily", "123456")
    };

    public AuthenticationController(IConfiguration configuration, IAuthenticationService service, IMapper mapper)
    {
        _configuration = configuration;
        _service = service;
        _mapper = mapper;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> SigninCredential(UserAuthentication userAuthentication)
    {
        if (userAuthentication != null! && userAuthentication.Username != null! && userAuthentication.Password != null!)
        {
            var user = _service.SigninCredentials(_mapper.Map<UserAuthentication, UserEntity>(userAuthentication));
            if (user != null!)
            {
                var token = _service.GenerateJwtToken(user.Result!);
                return Ok(new BaseResponse("Valid Credential!",
                    new TokenAuthentication(token, _service.GenerateRefreshToken())));
            }
            else
            {
                return BadRequest("Invalid Credential!");
            }
        }
        else
        {
            return BadRequest("Null username or password");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserAuthentication authentication)
    {
        try
        {
            await _service.Register(_mapper.Map<UserAuthentication, UserEntity>(authentication));
            return Created("", new BaseResponse("Register successfully!", null!));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, 500, "Problem", null);
        }
    }

    [HttpGet("get-accounts")]
    public async Task<IActionResult> GetAllAccounts()
    {
        var users = await _service.GetAllUsers();
        if (!users.Any())
        {
            return NotFound(new BaseResponse("Not found!", null!));
        }
        return Ok(new BaseResponse("Successful", users));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> GenerateNewToken(TokenAuthentication tokenAuthentication)
    {
        if (tokenAuthentication.RefreshToken.Expired < DateTime.Now)
        {
            return Unauthorized("Token expired.");
        }
        else
        {
            var user = _service.GetUserWithUsername(_service.GetPrincipalFromExpiredToken(tokenAuthentication.Token).Username).Result;
            user.RefreshToken = _service.GenerateRefreshToken().Token;
            
            return Ok(user);
        }
        return Ok("hehe");
    }

}