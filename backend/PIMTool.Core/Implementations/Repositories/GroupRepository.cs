using System;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Implementations.Repositories.Base;
using PIMTool.Core.Interfaces.Repositories;

namespace PIMTool.Core.Implementations.Repositories;

public class GroupRepository : Repository<Group, Guid>, IGroupRepository
{
    public GroupRepository(IAppDbContext appDbContext) : base(appDbContext)
    {
    }
}