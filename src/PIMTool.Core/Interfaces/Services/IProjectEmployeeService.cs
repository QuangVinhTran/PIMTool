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
        IQueryable<ProjectEmployee> GetAll();
        Task AddAsync (ProjectEmployee projectEmployee, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<ProjectEmployee> projectEmployee, CancellationToken cancellationToken = default);
        void DeleteRange (params ProjectEmployee[] projectEmployees);
    }
}
