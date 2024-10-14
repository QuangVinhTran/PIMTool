using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using PIMTool.Entities;
using PIMTool.Payload.Request.Authentication;
using PIMTool.Repositories;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PIMTool.Services;

public interface IAuthenticationService
{
    public Claim? GetRole(ClaimsPrincipal claimsPrincipal);
    public Task<ICollection<UserEntity>> GetAllUsers();
    public Task<UserEntity?> SigninCredentials(UserEntity entity);
    public Task Register(UserEntity entity);
    public RefreshToken GenerateRefreshToken();
    public string GenerateJwtToken(UserEntity entity);
    public UserEntity GetPrincipalFromExpiredToken(string token);
    public Task<UserEntity?> GetUserWithUsername(string username);
}

public class AuthenticationService : IAuthenticationService
{
    private UserRepository _repository;
    private IMapper _mapper;
    private IConfiguration _configuration;

    public AuthenticationService(UserRepository userRepository, IMapper mapper, IConfiguration configuration)
    {
        _repository = userRepository;
        _mapper = mapper;
        _configuration = configuration;
    }
    
    public Claim? GetRole(ClaimsPrincipal claimsPrincipal)
    {
        var role = claimsPrincipal.Claims.FirstOrDefault(x =>
            x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase));
        return role;
    }

    public async Task<ICollection<UserEntity>> GetAllUsers()
    {
        var users = await _repository.GetAllEntity();
        return users.ToList();
    }

    public Task<UserEntity?> SigninCredentials(UserEntity entity)
    {
        var user = _repository.GetEmployeeWithAuthentication(entity);
        return user;
    }


    public async Task Register(UserEntity entity)
    {
        var refreshToken = GenerateRefreshToken();
        entity.RefreshToken = refreshToken.Token;
        entity.TokenCreated = refreshToken.Created;
        entity.TokenExpires = refreshToken.Expired;
        await _repository.InsertNewEntity(entity);
    }

    public RefreshToken GenerateRefreshToken()
    {
        var token = new RefreshToken()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expired = DateTime.UtcNow.AddMinutes(7),
            Created = DateTime.UtcNow
        };
        return token;
    }

    public string GenerateJwtToken(UserEntity entity)
    {
        //Create claims details based on the user information
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.Day.ToString()),
            new Claim("UserName", entity.Username)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials: signIn
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public UserEntity GetPrincipalFromExpiredToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var username = handler.ReadJwtToken(token);
        return new UserEntity(username.Payload.Claims.ToList()[3].Value);
    }

    public async Task<UserEntity?> GetUserWithUsername(string username)
    {
        var users = await _repository.GetAllEntity();
        return users.First(user => user.Username == username);
    }

    public async Task UpdateUser(UserEntity entity)
    {
        await _repository.UpdateEntity(entity);
    }
}