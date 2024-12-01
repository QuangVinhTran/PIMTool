using System.Linq.Expressions;

namespace PIMTool.Core.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> Get(Expression<Func<T, object>>? expression = null);

        Task<T?> GetAsync(int id, Expression<Func<T, object>>? expression = null, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task Delete(T entity);
        Task DeleteRange(params T[] entities);
        Task Update(T entity);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression);
        void ClearTrackers();
    }
}