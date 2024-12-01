using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;

namespace PIMTool.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public async Task<bool> Login(string username, string password)
        {
            if(username == "Admin" && password == "1")
            {
                return true;
            }
            return false;
        }
    }
}
