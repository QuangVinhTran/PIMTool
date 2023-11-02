using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;

namespace PIMTool.Services
{
    public class ProjectEmployeeService : IProjectEmployeeService
    {
        private readonly IRepository<ProjectEmployee> _repository;

        public ProjectEmployeeService(IRepository<ProjectEmployee> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(ProjectEmployee projectEmployee)
        {
            await _repository.AddAsync(projectEmployee);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProjectEmployee projectEmployee)
        {
            _repository.Delete(projectEmployee);
            await _repository.SaveChangesAsync();
        }

        public async Task<ProjectEmployee> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<IEnumerable<ProjectEmployee>> GetProjectEmployees()
        {
            var entities = await _repository.Get().ToListAsync();
            return entities;
        }

        public async Task UpdateAsync()
        {
            await _repository.SaveChangesAsync();
        }
    }
}
