using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PIMTool.Core.Constants;
using PIMTool.Core.Helpers;
using PIMTool.Core.Interfaces;
using PIMTool.Core.Interfaces.Repositories;

namespace PIMTool.Core.Implementations.Repositories;

public class AppDbContext : DbContext, IAppDbContext
{
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public AppDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }
        
        var connectionString = DataAccessHelper.GetDefaultConnection();
        optionsBuilder.UseSqlServer(connectionString,options =>
                options.CommandTimeout(DataAccessConstants.DEAULT_COMMAND_TIMEOUT_IN_SECONDS))
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var types = ReflectionHelper.GetClassesFromAssignableType(typeof(IModelMapper));
        foreach (var t in types)
        {
            var instance = Activator.CreateInstance(t) as IModelMapper;
            instance?.Mapping(modelBuilder);
        }
    }
    
    public DbSet<T> CreateSet<T>() where T : class
    {
        return base.Set<T>();
    }
    
    public new void Attach<T>(T item) where T : class
    {
        base.Entry<T>(item).State = EntityState.Unchanged;
    }
    
    public void SetModified<T>(T item) where T : class
    {
        base.Entry<T>(item).State = EntityState.Modified;
    }

    public void SetDeleted<T>(T item) where T : class
    {
        base.Entry(item).State = EntityState.Deleted;
    }

    public void Refresh<T>(T item) where T : class
    {
        base.Entry<T>(item).Reload();
    }
    
    public int ExecuteSqlRaw(string sql, params object[] parameters)
    {
        // return Database.ExecuteSqlRaw(sql, parameters);
        return 0;
    }
    
    public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
    {
        return 0;
        // return await Database.ExecuteSqlRawAsync(sql, parameters);
    }

    public new void Update<T>(T entity)
    {
        base.Update(entity!);
    }

    public new void SaveChanges()
    {
        base.SaveChanges();
    }
    
    public async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}