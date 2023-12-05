using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using PIMTool.Dtos;

namespace PIMTool.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _repository;

        public EmployeeService(IRepository<Employee> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Employee emp)
        {
            await _repository.AddAsync(emp);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee emp)
        {
            _repository.Delete(emp);
            await _repository.SaveChangesAsync();
        }

        public async Task<Employee> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            var entities = await _repository.Get().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<Employee>> Search(string searchText)
        {
            var formattedSearchText = searchText.ToLower().Trim();
            return await _repository.Get()
                .Where(e => e.Visa.ToLower().Contains(formattedSearchText) || e.FirstName.ToLower().Contains(formattedSearchText) || e.LastName.ToLower().Contains(formattedSearchText))
                .ToListAsync();
        }

        public async Task UpdateAsync()
        {
            await _repository.SaveChangesAsync();
        }
    }
}
