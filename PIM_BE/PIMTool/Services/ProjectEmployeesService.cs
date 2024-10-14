using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;

namespace PIMTool.Services;

public class ProjectEmployeesService : IProjectEmployeesService
{
    //IRepository<ProjectEmployee> _projectEmployeeRepository;
    IRepository<Employee> _employeeRepository;

    public ProjectEmployeesService(IRepository<Employee> employeeRepository)
    {
        //_projectEmployeeRepository = projectEmployeeRepository;
        _employeeRepository = employeeRepository;
    }

    public Task EmployeeToProject(int projectId, List<Employee> listEmployee)
    {
        throw new NotImplementedException();
        //foreach (var e in listEmployee)
        //{
        //}
    }

    public Task<List<Employee>> GetEmpoyeeFromProject(int projectId)
    {
        throw new NotImplementedException();
    }

    //public async Task<List<Employee>> GetEmpoyeeFromProject(int projectId)
    //{
    //    List<ProjectEmployee> listProjectEmployee = (List<ProjectEmployee>)await _projectEmployeeRepository.GetAll();
    //    listProjectEmployee = listProjectEmployee.Where(o => o.ProjectId == projectId).ToList();
    //    List<Employee> listEmployee = new List<Employee>();
    //    foreach(var o in listProjectEmployee)
    //    {
    //        listEmployee.Add(o.Employee);
    //    }
    //    return listEmployee;
    //}
}
