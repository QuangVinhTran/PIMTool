using System.Linq.Expressions;
using Application.Commons;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
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

        public IQueryable<T> Get()
        {
            return _set.Where(x => true);
        }
        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _set.ToListAsync(cancellationToken);
        }

        public async Task<T?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await Get().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _set.AddRangeAsync(entities, cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _set.AddAsync(entity, cancellationToken);
        }

        public void Update(T entity, CancellationToken cancellationToken = default)
        {
            _set.Attach(entity);
            _pimContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(params T[] entities)
        {
            _set.RemoveRange(entities);
        }

        public IQueryable<T> Search(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate);
        }

        public async Task<Pagination<T>> ToPagination(int pageIndex = 0, int pageSize = 5, CancellationToken cancellationToken = default)
        {
            var itemCount = await _set.CountAsync(cancellationToken);
            var items = await _set.Skip(pageIndex * pageSize)
                                  .Take(pageSize)
                                  .AsNoTracking()
                                  .ToListAsync(cancellationToken);

            var result = new Pagination<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

    }
}