using PIMTool.Core.Domain.Entities;

namespace PIMTool.Core.Interfaces.Services;

public interface IGroupSevice
{
    Task<IEnumerable<Group>> GetAll();
    Task Create(Group group);
    Task ChooseLeaderGroup(int groupId, int leaderId);
}
