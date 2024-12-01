using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Exceptions;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;

namespace PIMTool.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _repository;

        public EmployeeService(IRepository<Employee> repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            await _repository.AddAsync(employee, cancellationToken);
            await _repository.SaveChangesAsync();
        }

        public async Task<Employee?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                Employee employee = await _repository.GetAsync(id);
                if (employee == null)
                {
                    throw new BusinessException($"Employee with ID {id} not found.");
                }

                return employee;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Business error: {ex.Message}", ex);
            }
        }
    }
}
