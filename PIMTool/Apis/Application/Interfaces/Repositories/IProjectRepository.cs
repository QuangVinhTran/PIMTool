using Application.ViewModels.EmployeeViewModels;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        bool IsProjectNumberExists(int projectNumber);
        Task<IEnumerable<Project>> SearchProjectAsync(string name, int? projectNumber, string? customer, StatusEnum? status);
        Task<IEnumerable<Employee>> GetEmployeInProjectAsync(int projectId);
        Task<IEnumerable<Project>> SearchProjectAsync(string searchTerm);
        Task<IEnumerable<Project>> GetProjectsByStatusAsync(StatusEnum status);
        Task<IEnumerable<Project>> FilterProjectsAsync(string searchTerm, StatusEnum? status);
    }
}