using System.Threading.Tasks;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Models;

namespace PIMTool.Core.Interfaces.Services;

public interface IJwtService
{
    string GenerateAccessToken(PIMUser user);
    Task<string> GenerateRefreshToken(dynamic userId, string ipAddress);
    Task<ApiActionResult> RefreshToken(string refreshToken, string ipAddress);
}