using Application.Commons;
using Application.Commons.ServiceResponse;
using Application.ViewModels.EmployeeViewModels;
using Application.ViewModels.ProjectViewModels;
using Domain.Enums;

namespace Application.Interfaces.Repositories
{
    public interface IProjectService
    {
        bool IsProjectNumberExists(int projectNumber);
        Task<ServiceResponse<IEnumerable<ProjectViewModel>>> GetProjects();
        Task<ServiceResponse<ProjectViewModel>> GetProjectById(int id, CancellationToken cancellationToken = default);
        Task<ServiceResponse<CreateProjectViewModel>> CreateProject(CreateProjectViewModel model, CancellationToken cancellationToken = default);
        Task<ServiceResponse<UpdateProjectViewModel>> UpdateProject(int id, UpdateProjectViewModel model, CancellationToken cancellationToken = default);
        Task<ServiceResponse<bool>> DeleteProject(int id, CancellationToken cancellationToken = default);
        Task<ServiceResponse<bool>> DeleteProjects(int[] idProjects, CancellationToken cancellationToken = default);
        Task<Pagination<ProjectViewModel>> SearchProjectAsync(string searchTerm, int pageIndex = 0, int pageSize = 5, CancellationToken cancellationToken = default);
        Task<ServiceResponse<IEnumerable<EmployeeViewModel>>> GetEmployeeInProject(int id, CancellationToken cancellationToken = default);
        Task<Pagination<ProjectViewModel>> GetProjectsByStatus(StatusEnum status, int pageIndex = 0, int pageSize = 5);
        Task<Pagination<ProjectViewModel>> GetProjectPagingAsync(int pageIndex = 0, int pageSize = 5, CancellationToken cancellationToken = default);
        Task<Pagination<ProjectViewModel>> FilterProjectsAsync(string searchTerm, StatusEnum? status, int pageIndex, int pageSize = 5, CancellationToken cancellationToken = default);
    }
}