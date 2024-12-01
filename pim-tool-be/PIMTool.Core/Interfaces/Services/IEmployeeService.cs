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
        Task AddAsync(Employee emp);
        Task DeleteAsync(Employee emp);
        Task<Employee> GetAsync(int id);
        Task<IEnumerable<Employee>> GetEmployees();
        Task<IEnumerable<Employee>> Search(string searchText);
        Task UpdateAsync();
    }
}
