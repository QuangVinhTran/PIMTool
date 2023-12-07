using System.Threading.Tasks;
using PIMTool.Core.Models;

namespace PIMTool.Core.Interfaces.Services;

public interface IAuthenticationService
{
    Task<ApiActionResult> LoginAsync(UserLoginModel userLogin, string ipAddress);
    Task<ApiActionResult> RegisterAsync(UserRegisterModel userRegister);
}