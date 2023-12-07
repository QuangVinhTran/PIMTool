using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ProjectEmployeeRepository : IProjectEmployeeRepository
    {
        private readonly PimContext _pimContext;
        private readonly DbSet<ProjectEmployee> _set;

        public ProjectEmployeeRepository(PimContext pimContext)
        {
            _pimContext = pimContext;
            _set = _pimContext.Set<ProjectEmployee>();
        }

        public async Task AddAsync(ProjectEmployee entity, CancellationToken cancellationToken = default)
        {
            await _set.AddAsync(entity, cancellationToken);
        }


        public async Task<IEnumerable<ProjectEmployee>> SearchProjectEmployeeById(int searchTerm, CancellationToken cancellationToken = default)
        {
            IQueryable<ProjectEmployee> query = _pimContext.ProjectEmployees;

            query = query.Where(x => x.ProjectId == searchTerm);

            return await query.ToListAsync();
        }

        public async Task Delete(IEnumerable<int> ids, CancellationToken cancellationToken = default)
        {
            var entitiesToRemove = await _set.Where(x => ids.Contains(x.EmployeeId)).ToListAsync(cancellationToken);
            _set.RemoveRange(entitiesToRemove);
        }

    }
}