using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Core.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<bool> Login(string username, string password);
    }
}
