using AutoMapper;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Extensions;

namespace PIMTool.Services;

public class GroupService : IGroupSevice
{
    IRepository<Group> _repoGroup;
    IRepository<Employee> _repoEmployee;

    public GroupService(IRepository<Group> repoGroup, IRepository<Employee> repoEmployee, IMapper mapper)
    {
        _repoGroup = repoGroup;
        _repoEmployee = repoEmployee;
    }

    public async Task ChooseLeaderGroup(int groupId, int leaderId)
    {
        Group? group = await _repoGroup.GetAsync(groupId);
        IEnumerable<Group> listGroup = await _repoGroup.GetAll();
        Employee? employee = await _repoEmployee.GetAsync(groupId);
        bool checkNoLeader = false;
        if (employee != null)
        {
           checkNoLeader = employee.NoLeaderGroup(listGroup);
        }

        if (group != null && employee != null && checkNoLeader)
        {
            group.GroupLeaderId = leaderId;
        }
        else
        {
            throw new Exception($"Not found");
        }
    }

    public async Task Create(Group group)
    {
        await _repoGroup.AddAsync(group);
        await _repoGroup.SaveChangesAsync();
    }

    public async Task<IEnumerable<Group>> GetAll()
    {
        return await _repoGroup.GetAll();
    }
}
