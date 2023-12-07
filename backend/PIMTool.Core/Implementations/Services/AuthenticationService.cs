using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using PIMTool.Core.Constants;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Exceptions;
using PIMTool.Core.Helpers;
using PIMTool.Core.Implementations.Services.Base;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Core.Models;
using PIMTool.Core.Models.Response;

namespace PIMTool.Core.Implementations.Services;

public class AuthenticationService : BaseService, IAuthenticationService
{
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    private readonly IPIMUserRepository _pimUserRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public AuthenticationService(ILifetimeScope scope) : base(scope)
    {
        _jwtService = Resolve<IJwtService>();
        _mapper = Resolve<IMapper>();
        _pimUserRepository = Resolve<IPIMUserRepository>();
        _refreshTokenRepository = Resolve<IRefreshTokenRepository>();
        _unitOfWork = Resolve<IUnitOfWork>();
    }

    public async Task<ApiActionResult> LoginAsync(UserLoginModel userLogin, string ipAddress)
    {
        var user = (await _pimUserRepository.FindByAsync(u =>
            u.Email == userLogin.Email)).FirstOrDefault();
        if (user is null)
        {
            throw new UserDoesNotExistException();
        }
        if (!EncryptionHelper.Verify(userLogin.Password, user.Password))
        {
            throw new InvalidCredentialsException();
        }
        
        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = await _jwtService.GenerateRefreshToken(user.Id, ipAddress);
        var tokenResponse = new TokenResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTimeOffset.Now.AddHours(TokenConstants.ACCESS_TOKEN_LIFETIME)
        };
        return new ApiActionResult(true, "Logged in successfully"){Data = tokenResponse};
    }

    public async Task<ApiActionResult> RegisterAsync(UserRegisterModel userRegister)
    {
        if (await _pimUserRepository.ExistsAsync(u => u.Email == userRegister.Email))
        {
            throw new UserAlreadyExistsException();
        }

        var user = _mapper.Map<PIMUser>(userRegister);
        user.SetCreatedInfo(Guid.Empty);
        await _pimUserRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();
        return new ApiActionResult(true, "Registered successfully");
    }
}