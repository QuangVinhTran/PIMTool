using PIMTool.Core.Domain.Entities;

namespace PIMTool.Core.Interfaces.Services
{
    public interface IProjectService
    {
        Task<Project?> GetAsync(int id, CancellationToken cancellationToken = default);

        Task AddAsync(Project entity, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        IQueryable<Project> Get();

        IEnumerable<Project> SearchProjectByProjectNumberOrNameOrCustomerAndStatus(string searchValue, string status, CancellationToken cancellationToken = default);

        Task<Project> GetByProjectNumber(int projectNumber, CancellationToken cancellationToken = default);
        Task DeleteProjects(Project[] projects);

    }
}