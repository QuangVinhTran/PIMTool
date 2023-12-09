using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IProjectEmployeeRepository
    {
        Task AddAsync(ProjectEmployee entity, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProjectEmployee>> SearchProjectEmployeeById(int searchTerm, CancellationToken cancellationToken = default);
        Task Delete(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    }
}