using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PIMTool.Core.Constants;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Exceptions;
using PIMTool.Core.Implementations.Services.Base;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Core.Models;
using PIMTool.Core.Models.Settings;

namespace PIMTool.Core.Implementations.Services;

public class JwtService : BaseService, IJwtService
{
    
    private readonly JwtSettings _jwtSettings;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPIMUserRepository _pimUserRepository;

    public JwtService(ILifetimeScope scope) : base(scope)
    {
        _jwtSettings = Resolve<IConfiguration>().GetSection(nameof(JwtSettings)).Get<JwtSettings>() ??
                       throw new MissingJwtSettingsException();
        _refreshTokenRepository = Resolve<IRefreshTokenRepository>();
        _unitOfWork = Resolve<IUnitOfWork>();
        _pimUserRepository = Resolve<IPIMUserRepository>();
    }

    public string GenerateAccessToken(PIMUser dtoUser)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, dtoUser.Email ?? ""),
            new Claim(ClaimTypes.GivenName, dtoUser.FirstName ?? ""),
            new Claim(ClaimTypes.Surname, dtoUser.LastName ?? ""),
            new Claim(ClaimTypes.DateOfBirth, dtoUser.BirthDate.ToShortDateString()),
            new Claim(ClaimTypes.Role, dtoUser.Role ?? ""),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            // expires: DateTime.Now.AddSeconds(15),
            expires: DateTime.Now.AddHours(TokenConstants.ACCESS_TOKEN_LIFETIME),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshToken(dynamic userId, string ipAddress)
    {
        var refreshToken = new RefreshToken()
        {
            Token = Guid.NewGuid(),
            UserId = userId,
            ValidUntil = DateTime.Now.AddHours(TokenConstants.REFRESH_TOKEN_LIFETIME),
            IPAddress = ipAddress,
            CreatedAt = DateTime.Now
        };
        await _refreshTokenRepository.AddAsync(refreshToken);
        await _unitOfWork.CommitAsync();
        return refreshToken.Token.ToString();
    }

    public async Task<ApiActionResult> RefreshToken(string refreshToken, string ipAddress)
    {
        Guid.TryParse(refreshToken, out var token);
        if (token == default)
        {
            return BuildErrorResult("Invalid refresh token");
        }

        var retrievedToken = await _refreshTokenRepository.GetAsync(t => t.Token == token);
        if (retrievedToken is null)
        {
            throw new RefreshTokenDoesNotExistException();
        }

        if (retrievedToken.ValidUntil < DateTime.Now)
        {
            return BuildErrorResult("Refresh token expired");
        }

        if (retrievedToken.IPAddress != ipAddress)
        {
            return BuildErrorResult("Cannot refresh token from a strange address");
        }

        var retrievedUser = await _pimUserRepository.GetAsync(u => u.Id == retrievedToken.UserId);
        if (retrievedUser == default)
        {
            return BuildErrorResult("Invalid user");
        }

        var newAccessToken = GenerateAccessToken(retrievedUser);
        return new ApiActionResult(true, "Token refreshed successfully") { Data = newAccessToken };
    }

    private ApiActionResult BuildErrorResult(string detail)
    {
        return new ApiActionResult(false, detail);
    }
}