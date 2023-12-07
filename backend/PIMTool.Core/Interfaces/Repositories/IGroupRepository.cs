using System;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories.Base;

namespace PIMTool.Core.Interfaces.Repositories;

public interface IGroupRepository : IRepository<Group, Guid>
{
    
}