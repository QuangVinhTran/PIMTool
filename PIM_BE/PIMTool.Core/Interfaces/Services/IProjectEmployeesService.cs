using PIMTool.Core.Domain.Entities;

namespace PIMTool.Core.Interfaces.Services;

public interface IProjectEmployeesService
{
    Task EmployeeToProject(int projectId, List<Employee> listEmployee);
    Task<List<Employee>> GetEmpoyeeFromProject(int projectId); 
}
