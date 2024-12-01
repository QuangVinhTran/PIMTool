using System.Linq.Expressions;
using Application.Commons;

namespace Application.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        Task<T?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity, CancellationToken cancellationToken = default);
        IQueryable<T> Search(Expression<Func<T, bool>> predicate);
        void Delete(params T[] entities);
        Task<Pagination<T>> ToPagination(int pageIndex = 0, int pageSize = 5, CancellationToken cancellationToken = default);
    }
}