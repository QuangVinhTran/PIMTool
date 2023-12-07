using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PIMTool.Core.Interfaces.Repositories;

public interface IAppDbContext
{
    DbSet<T> CreateSet<T>() where T : class;
    void Attach<T>(T item) where T : class;
    void SetModified<T>(T item) where T : class;
    void SetDeleted<T>(T item) where T : class;
    void Refresh<T>(T item) where T : class;
    int ExecuteSqlRaw(string sql, params object[] parameters);
    Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);
    void Update<T>(T entity);
    void SaveChanges();
    Task SaveChangesAsync();
}