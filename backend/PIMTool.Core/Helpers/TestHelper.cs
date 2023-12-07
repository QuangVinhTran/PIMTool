using Autofac;
using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Implementations.Services.Base;
using PIMTool.Core.Interfaces.Repositories;

namespace PIMTool.Core.Helpers;

public class TestHelper : BaseService
{
    private readonly DbContext _dbContext;
    public TestHelper(ILifetimeScope scope) : base(scope)
    {
        _dbContext = (DbContext)Resolve<IAppDbContext>();
    }

    public void Migrate()
    {
        _dbContext.Database.Migrate();
    }
}