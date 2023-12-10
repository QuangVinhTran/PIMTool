using PIMTool.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Core.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
        Task<Employee?> GetAsync(int id, CancellationToken cancellationToken = default);
    }
}
