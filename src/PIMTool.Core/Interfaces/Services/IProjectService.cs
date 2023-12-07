using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Domain.Objects;

namespace PIMTool.Core.Interfaces.Services
{
    public interface IProjectService
    {
        IQueryable<Project> Get(ProjectParameters projectParameters);
        Task<Project?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Project project, List<string> employeeList, CancellationToken cancellationToken = default);
        Task Delete(Project project);
        Task DeleteRange(params Project[] projects);
        Task Update(Project project, List<string> employeeVisaList);
    }
}