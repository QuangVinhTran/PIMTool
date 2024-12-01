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
        IQueryable<Employee> Get();
        Task<Employee?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<ICollection<Employee>> SearchEmployeeByProjId(int projectId, CancellationToken cancellationToken = default);
        Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
        Task Update(Employee employee);
        List<string> CheckNonExistentVisa(string visaList);
    }
}
