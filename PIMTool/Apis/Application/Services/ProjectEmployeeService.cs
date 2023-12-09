using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class ProjectEmployeeService : IProjectEmployeeService
    {
        public Task AddAsync(ProjectEmployee entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}