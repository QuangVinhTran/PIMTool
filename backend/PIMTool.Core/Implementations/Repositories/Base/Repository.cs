using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities.Base;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Repositories.Base;
using PIMTool.Core.Models;

namespace PIMTool.Core.Implementations.Repositories.Base;

public abstract class Repository<T, TKey> : IRepository<T, TKey> where T : Entity<TKey>
{
    protected readonly IAppDbContext _appDbContext;

    protected Repository(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    private DbSet<T> GetSet()
    {
        return _appDbContext.CreateSet<T>();
    }

    public void Add(T entity)
    {
        GetSet().Add(entity);
    }

    public async Task AddAsync(T entity)
    {
        await GetSet().AddAsync(entity).ConfigureAwait(false);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        GetSet().AddRange(entities);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await GetSet().AddRangeAsync(entities).ConfigureAwait(false);
    }

    public void Update(T entity)
    {
        _appDbContext.Update(entity);
    }

    public Task UpdateAsync(T entity)
    {
        _appDbContext.SetModified(entity);
        return Task.CompletedTask;
    }

    public void UpdateMany(T entities)
    {
        throw new NotImplementedException();
    }

    public Task UpdateManyAsync(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> GetAll()
    {
        return GetSet().AsNoTracking();
    }

    public async Task<IQueryable<T>> GetAllAsync()
    {
        return await Task.FromResult(GetSet().AsNoTracking()).ConfigureAwait(false);
    }

    public T? Get(object id)
    {
        return GetSet().AsNoTracking().FirstOrDefault(e => string.Equals(e.Id!.ToString(), id.ToString(), StringComparison.OrdinalIgnoreCase));
    }

    public async Task<T?> GetAsync(object id)
    {
        return await GetSet().AsNoTracking().FirstOrDefaultAsync(e => string.Equals(e.Id!.ToString(), id.ToString(), StringComparison.OrdinalIgnoreCase));
    }

    public T Get(Expression<Func<T, bool>> specification)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> specification)
    {
        return await GetSet().FirstOrDefaultAsync(specification);
    }

    public IQueryable<T> FindBy(Expression<Func<T, bool>> specification)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<T>> FindByAsync(Expression<Func<T, bool>> specification)
    {
        return Task.FromResult(GetSet().Where(specification));
    }

    public long Count(Expression<Func<T, bool>> specification)
    {
        return GetSet().Count();
    }

    public async Task<long> CountAsync(Expression<Func<T, bool>> specification)
    {
        return await GetSet().CountAsync();
    }

    public bool Exists(Expression<Func<T, bool>> specification)
    {
        return GetSet().Where(specification).ToList().Count > 0;
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> specification)
    {
        return (await GetSet().Where(specification).ToListAsync()).Count > 0;
    }

    public IQueryable<T> GetFilteredElements(Expression<Func<T, bool>> specification, long pageIndex, int pageSize, IList<SortByInfo> orderByExpression = null)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<T>> GetFilteredElementsAsync(Expression<Func<T, bool>> specification, long pageIndex, int pageSize, IList<SortByInfo> orderByExpression = null)
    {
        throw new NotImplementedException();
    }

    public void Delete(object id)
    {
        var entity = GetSet().First(e => Equals(e.Id, (TKey)id));
        _appDbContext.SetDeleted(entity);
    }

    public async Task DeleteAsync(object id)
    {
        var entity = await GetSet().FirstAsync(e => Equals(e.Id, (TKey)id));
        _appDbContext.SetDeleted(entity);
    }

    public void DeleteMany(Expression<Func<T, bool>> specification)
    {
        foreach (var entity in GetSet().Where(specification))
        {
            entity.IsDeleted = true;
        }
    }

    public async Task DeleteManyAsync(Expression<Func<T, bool>> specification)
    {
        await GetSet().Where(specification).ForEachAsync(e => e.IsDeleted = true);
    }
}