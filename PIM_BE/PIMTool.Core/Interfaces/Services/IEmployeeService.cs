using PIMTool.Core.Domain.Entities;

namespace PIMTool.Core.Interfaces.Services;

public interface IEmployeeService
{
    Task<Employee?> GetAsync(int id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task Create(Employee employee);
    Task Update(Employee employee);
    Task Delete(Employee employee);
    Task Delete(int id);
}
