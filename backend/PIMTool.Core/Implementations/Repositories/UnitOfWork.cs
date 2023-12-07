using System;
using System.Threading.Tasks;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;

namespace PIMTool.Core.Implementations.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IAppDbContext _dbContext;
    private readonly ILoggerService _logger;

    public UnitOfWork(IAppDbContext dbContext, ILoggerService logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public void Commit()
    {
        _dbContext.SaveChanges();
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Rollback()
    {
        _logger.Error("Transaction rollback - Do nothing");
    }

    public async Task RollbackAsync()
    {
        _logger.Error("Transaction rollback - Do nothing");
        await Task.CompletedTask;
    }
    
    public void Dispose()
    {
        try
        {
            GC.SuppressFinalize(this);
        }
        catch (Exception e)
        {
            _logger.Error(e);
        }
    }
}