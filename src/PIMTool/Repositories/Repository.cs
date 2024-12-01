using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Database;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PIMTool.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly PimContext _pimContext;
        private readonly DbSet<T> _set;

        public Repository(PimContext pimContext)
        {
            _pimContext = pimContext;
            _set = _pimContext.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T, object>>? expression = null)
        {
            if (expression != null)
                return _set.Where(x => true).Include(expression);
            return _set.Where(x => true);
        }

        public async Task<T?> GetAsync(int id, Expression<Func<T, object>>? expression = null, CancellationToken cancellationToken = default)
        {
            return await Get(expression).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _set.AddRangeAsync(entities, cancellationToken);
        }
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _set.AddAsync(entity, cancellationToken);
        }

        public async Task Delete(T entity)
        {
            _set.Remove(entity);
        }

        public async Task DeleteRange(params T[] entities)
        {
            _set.RemoveRange(entities);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _pimContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _set.Where(expression);
        }

        public async Task Update(T entity)
        {
            _set.Update(entity);
        }

        public void ClearTrackers()
        {
            _pimContext.ChangeTracker.Clear();
        }
    }
}