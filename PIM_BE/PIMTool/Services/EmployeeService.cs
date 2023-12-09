using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;

namespace PIMTool.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IRepository<Employee> _repository;

    public EmployeeService(IRepository<Employee> repository)
    {
        _repository = repository;
    }

    public async Task Create(Employee employee)
    {
        await _repository.AddAsync(employee);
        await _repository.SaveChangesAsync();
    }

    public async Task Delete(Employee employee)
    {
        _repository.Delete(employee);
        await _repository.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var employee = await _repository.GetAsync(id);
        if (employee != null)
        {
            _repository.Delete(employee);
            await _repository.SaveChangesAsync();
        } else
        {
            throw new Exception($"Not found employeeId {id}");
        }
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _repository.GetAll();
    }

    public async Task<Employee?> GetAsync(int id)
    {
        var employee = await _repository.GetAsync(id);
        if (employee != null)
        {
            return employee;
        }
        return null;
    }

    public async Task Update(Employee employee)
    {
        var existing = await _repository.GetAsync(employee.Id);
        if (existing != null)
        {
            existing.Visa = employee.Visa;
            existing.FirstName = employee.FirstName;
            existing.LastName = employee.LastName;
            existing.FirstName = existing.FirstName;
            existing.Birthday = existing.Birthday;
            await _repository.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"Not found employeeId {employee.Id}");
        }
    }
}
