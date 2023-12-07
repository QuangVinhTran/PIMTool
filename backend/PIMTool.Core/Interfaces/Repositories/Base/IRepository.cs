using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PIMTool.Core.Domain.Entities.Base;
using PIMTool.Core.Models;

namespace PIMTool.Core.Interfaces.Repositories.Base;

public interface IRepository<T, TKey> where T : Entity<TKey>
{
    void Add(T entity);
    Task AddAsync(T entity);
    void AddRange(IEnumerable<T> entities);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    Task UpdateAsync(T entity);
    void UpdateMany(T entities);
    Task UpdateManyAsync(IEnumerable<T> entities);
    IQueryable<T> GetAll();
    Task<IQueryable<T>> GetAllAsync();
    T? Get(object id);
    Task<T?> GetAsync(object id);
    T Get(Expression<Func<T, bool>> specification);
    Task<T?> GetAsync(Expression<Func<T, bool>> specification);
    IQueryable<T> FindBy(Expression<Func<T, bool>> specification);
    Task<IQueryable<T>> FindByAsync(Expression<Func<T, bool>> specification);
    long Count(Expression<Func<T, bool>> specification);
    Task<long> CountAsync(Expression<Func<T, bool>> specification);
    bool Exists(Expression<Func<T, bool>> specification);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> specification);
    IQueryable<T> GetFilteredElements(Expression<Func<T, bool>> specification, long pageIndex, int pageSize,
        IList<SortByInfo> orderByExpression = null);
    Task<IQueryable<T>> GetFilteredElementsAsync(Expression<Func<T, bool>> specification, long pageIndex, int pageSize,
        IList<SortByInfo> orderByExpression = null);
    void Delete(object id);
    Task DeleteAsync(object id);
    void DeleteMany(Expression<Func<T, bool>> specification);
    Task DeleteManyAsync(Expression<Func<T, bool>> specification);
}