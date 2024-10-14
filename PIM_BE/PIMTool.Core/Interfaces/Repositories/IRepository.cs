using System.Threading.Tasks;

namespace PIMTool.Core.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        Task<IEnumerable<T>> GetAll();

        Task<T?> GetAsync(int id);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        void Delete(params T[] entities);
        void ClearChangeTracker();
        void Update(T entity);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        void SaveChange();
        Task<T?> GetUpdate(int id);
    }
}