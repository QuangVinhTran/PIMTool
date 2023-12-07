using PIMTool.Core.Exceptions;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Core.Models;

namespace PIMTool.Test;

public class AuthenticationServiceTest : BaseTest
{
    [Test]
    public async Task LoginAsync_Success()
    {
        // Arrange
        var authService = ResolveService<IAuthenticationService>();
        var userLogin = new UserLoginModel()
        {
            Email = "pimuser@pimtool.com",
            Password = "password"
        };
        
        // Act
        var result = await authService.LoginAsync(userLogin, "");
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        });
    }
    
    [Test]
    public void LoginAsync_Failure_InvalidEmail()
    {
        // Arrange
        var authService = ResolveService<IAuthenticationService>();
        var userLogin = new UserLoginModel()
        {
            Email = "invalid@email.com",
            Password = "password"
        };
        
        // Act
        // Assert
        Assert.ThrowsAsync<UserDoesNotExistException>(async () => await authService.LoginAsync(userLogin, ""));
    }
    
    [Test]
    public void LoginAsync_Failure_InvalidPassword()
    {
        // Arrange
        var authService = ResolveService<IAuthenticationService>();
        var userLogin = new UserLoginModel()
        {
            Email = "pimuser@pimtool.com",
            Password = "invalid_password"
        };
        
        // Act
        // Assert
        Assert.ThrowsAsync<InvalidCredentialsException>(async () => await authService.LoginAsync(userLogin, ""));
    }

    [Test]
    public async Task RegisterAsync_Success()
    {
        // Arrange
        var authService = ResolveService<IAuthenticationService>();
        var userRegister = new UserRegisterModel()
        {
            Email = "user_register@pimtool.com",
            Password = "user_register_password",
            FirstName = "Mock",
            LastName = "User Register",
            BirthDate = DateTime.Now
        };

        // Act
        var result = await authService.RegisterAsync(userRegister);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
        });
    }
    
    [Test]
    public void RegisterAsync_Failure_UserEmailAlreadyExists()
    {
        // Arrange
        var authService = ResolveService<IAuthenticationService>();
        var userRegister = new UserRegisterModel()
        {
            Email = "pimuser@pimtool.com",
            Password = "user_register_password",
            FirstName = "Mock",
            LastName = "User Register",
            BirthDate = DateTime.Now
        };

        // Act
        // Assert
        Assert.ThrowsAsync<UserAlreadyExistsException>(async () => await authService.RegisterAsync(userRegister));
    }
}