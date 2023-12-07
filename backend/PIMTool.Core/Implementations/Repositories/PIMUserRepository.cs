using System;
using System.Linq.Expressions;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Domain.Entities.Base;
using PIMTool.Core.Implementations.Repositories.Base;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Models;

namespace PIMTool.Core.Implementations.Repositories;

public class PIMUserRepository : Repository<PIMUser, Guid>, IPIMUserRepository
{
    public PIMUserRepository(IAppDbContext appDbContext) : base(appDbContext)
    {
    }
}