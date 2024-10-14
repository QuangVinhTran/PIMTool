using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PIMTool.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace PIMTool.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ResponseDto _responseDto;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
        _responseDto = new ResponseDto();
    }
    [AllowAnonymous]
    [HttpPost]
    public ResponseDto Login(User user)
    {
        if(user.Username.Equals("admin") && user.Password.Equals("admin"))
        {
            var token = CreateToken(user);
            _responseDto.Data = token;
            return _responseDto;

        } else if (user.Username.Equals("user") && user.Password.Equals("user"))
        {
            var token = CreateToken(user);
            _responseDto.Data = token;
            return _responseDto;
        } else
        {
            _responseDto.isSuccess = false;
            _responseDto.Error = "incorrectUsernameOrPassword";
            return _responseDto;
        }
    }
    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, user.Username)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("Jwt:Secret").Value!));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            audience: "http://localhost:7099",
            issuer: "http://localhost:7099",
            signingCredentials: cred
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

}
