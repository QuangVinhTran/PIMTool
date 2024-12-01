namespace PIMTool.Core.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();

        Task<T?> GetAsync(int id, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        void Delete(params T[] entities);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}