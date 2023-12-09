using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IProjectEmployeeService
    {
        Task AddAsync(ProjectEmployee entity, CancellationToken cancellationToken = default);
    }
}