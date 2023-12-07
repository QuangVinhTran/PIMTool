using System;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Domain.Entities.Base;
using PIMTool.Core.Interfaces.Repositories.Base;

namespace PIMTool.Core.Interfaces.Repositories;

public interface IPIMUserRepository : IRepository<PIMUser, Guid>
{
}