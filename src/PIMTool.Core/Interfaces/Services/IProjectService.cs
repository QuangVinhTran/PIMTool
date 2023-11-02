using PIMTool.Core.Domain.Entities;

namespace PIMTool.Core.Interfaces.Services
{
    public interface IProjectService
    {
        Task AddAsync(Project project);
        Task DeleteAsync(Project project);
        Task<Project?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Project>> GetProjects();
        Task UpdateAsync();
    }
}