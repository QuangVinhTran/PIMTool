using Microsoft.EntityFrameworkCore;
using PIMTool.Entities;
using PIMTool.Payload.Request.Paging;

namespace PIMTool.Repositories;

public class BaseRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    protected BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IQueryable<T>> GetAllEntity()
    {
        try
        {
            return _context.Set<T>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}" + ex);
        }
    }

    public async Task<IQueryable<T>> GetAllEntityWithPaging(PagingParameter pagingParameter)
    {
        try
        {
            return _context.Set<T>()
                .Skip((pagingParameter.CurrentPage - 1) * pagingParameter.PageSize)
                .Take(pagingParameter.PageSize);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}");
        }
    }

    public async Task<T?> GetEntityById(int id)
    {
        try
        {
            return await _context.Set<T>().FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}" + ex);
        }
    }

    public async Task InsertNewEntity(T entity)
    {
        try
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}" + ex);
        }
    }

    public async Task UpdateEntity(T entity)
    {
        try
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}" + ex);
        }
    }

    public async Task DeleteEntity(T entity)
    {
        try
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}" + ex);
        }
    }
    
}