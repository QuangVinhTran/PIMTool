using PIMTool.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Core.Interfaces.Services
{
    public interface IProjectEmployeeService
    {
        Task AddAsync(ProjectEmployee projectEmployee);
        Task DeleteAsync(ProjectEmployee projectEmployee);
        Task<ProjectEmployee> GetAsync(int EmployeeId, int ProjectId);
        Task<IEnumerable<ProjectEmployee>> GetProjectEmployees();
        Task<int[]> GetProjectMembers(int id);
        Task UpdateAsync();
    }
}
