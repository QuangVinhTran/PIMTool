using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Repositories;

namespace PIMTool.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;

        public ProjectService(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<Project?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetAsync(id, cancellationToken);
            return entity;
        }

        public async Task AddAsync(Project entity, CancellationToken cancellationToken = default)
        {
            await _repository.AddAsync(entity, cancellationToken);
        }

        public IQueryable<Project> Get()
        {
            return _repository.Get();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _repository.SaveChangesAsync(cancellationToken);
        }

        public IEnumerable<Project> SearchProjectByProjectNumberOrNameOrCustomerAndStatus(string searchValue, string status, CancellationToken cancellationToken = default)
        {
            return _repository.SearchProjectByProjectNumberOrNameOrCustomerAndStatus(searchValue, status);
        }

        public async Task<Project> GetByProjectNumber(int projectNumber, CancellationToken cancellationToken = default)
        {
            return await _repository.GetByProjectNumber(projectNumber);
        }

        public async Task DeleteProjects(Project[] projects)
        {
            _repository.Delete(projects);
        }
    }
}