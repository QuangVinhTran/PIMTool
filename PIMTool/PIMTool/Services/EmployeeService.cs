using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using PIMTool.Entities;
using PIMTool.Payload.Request.Paging;
using PIMTool.Repositories;

namespace PIMTool.Services;

public interface IEmployeeService
{
    public Task<ICollection<EmployeeEntity>> GetAllEmployees();
    public Task<ICollection<EmployeeEntity>> GetAllEmployeesWithPaging(PagingParameter pagingParameter);
    public Task<EmployeeEntity?> GetEmployeeById(int id);
    public Task AddNewEmployee(EmployeeEntity entity);
    public Task UpdateEmployee(EmployeeEntity entity);
    public Task DeleteEmployee(EmployeeEntity entity);
}
public class EmployeeService : IEmployeeService
{
    private readonly IMapper _mapper;
    private readonly EmployeeRepository _repository;

    public EmployeeService(IMapper mapper, EmployeeRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    
    public async Task<ICollection<EmployeeEntity>> GetAllEmployees()
    {
        var employees = await _repository.GetAllEntity();
        return employees.ToList();
    }

    public async Task<ICollection<EmployeeEntity>> GetAllEmployeesWithPaging(PagingParameter pagingParameter)
    {
        var employees = await _repository.GetAllEntityWithPaging(pagingParameter);
        return employees.ToList();
    }

    public async Task<EmployeeEntity?> GetEmployeeById(int id)
    {
        return await _repository.GetEntityById(id);
    }

    public async Task AddNewEmployee(EmployeeEntity entity)
    {
        await _repository.InsertNewEntity(entity);
    }

    public async Task UpdateEmployee(EmployeeEntity entity)
    {
        await _repository.UpdateEmployee(entity);
    }

    public async Task DeleteEmployee(EmployeeEntity entity)
    {
        await _repository.DeleteEntity(entity);
    }
}