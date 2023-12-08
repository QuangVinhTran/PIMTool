using Application.ViewModels.EmployeeViewModels;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        bool IsProjectNumberExists(int projectNumber);
        Task<IEnumerable<Employee>> GetEmployeInProjectAsync(int projectId);
        Task<IEnumerable<Project>> FilterProjectsAsync(string? searchTerm, StatusEnum? status);
    }
}