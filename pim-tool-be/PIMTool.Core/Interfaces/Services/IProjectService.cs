using PIMTool.Core.Domain.Entities;

namespace PIMTool.Core.Interfaces.Services
{
    public interface IProjectService
    {
        Task<Project> AddAsync(Project project);
        Task DeleteAsync(Project project);
        Task<Project> GetAsync(int id);
        Task<Project> GetByProjectNumber(int projectNumber);
        Task<IEnumerable<Project>> GetProjects();
        Task<IEnumerable<Project>> GetProjectsPagination(int skip, int limit);
        Task<IEnumerable<Project>> Search(string? searchVal, int? status);
        Task<IEnumerable<Project>> SearchWithPagination(string? searchVal, int? status, int skip, int limit);
        Task UpdateAsync();
        Task<int> GetProjectsCount();
    }
}