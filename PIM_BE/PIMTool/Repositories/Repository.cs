using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Database;

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
       
        public IQueryable<T> Get()
        {
            return _set.Where(x => true);
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await Get().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            return await _set.FindAsync(id);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _set.AddRangeAsync(entities, cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _set.AddAsync(entity, cancellationToken);
        }

        public void Delete(params T[] entities)
        {
            _set.RemoveRange(entities);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _pimContext.SaveChangesAsync(cancellationToken);
        }
        public void SaveChange()
        {
            _pimContext.SaveChanges();
        }
        public void Update(T entity)
        {
            _set.Update(entity);
            //_pimContext.Update(entity);
        }
        public void ClearChangeTracker()
        {
            _pimContext.ChangeTracker.Clear();
        }
        public async Task<T?> GetUpdate(int id)
        {
            return await _set.FindAsync(id);
        }
    }
}