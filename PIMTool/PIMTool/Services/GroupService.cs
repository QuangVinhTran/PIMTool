using AutoMapper;
using PIMTool.Entities;
using PIMTool.Payload.Request.Paging;
using PIMTool.Repositories;

namespace PIMTool.Services;

public interface IGroupService
{
    public Task<ICollection<GroupEntity>> GetAllGroups();
    public Task<ICollection<GroupEntity>> GetAllGroupsWithPaging(PagingParameter pagingParameter);
    public Task<GroupEntity?> GetGroupById(int id);
    public Task AddNewGroup(GroupEntity entity);
    public Task UpdateGroup(GroupEntity entity);
    public Task DeleteGroup(GroupEntity entity);
}
public class GroupService : IGroupService
{
    private readonly IMapper _mapper;
    private readonly GroupRepository _repository;

    public GroupService(IMapper mapper, GroupRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    
    public async Task<ICollection<GroupEntity>> GetAllGroups()
    {
        var groups = await _repository.GetAllEntity();
        return groups.ToList();
    }

    public async Task<ICollection<GroupEntity>> GetAllGroupsWithPaging(PagingParameter pagingParameter)
    {
        var groups = await _repository.GetAllEntityWithPaging(pagingParameter);
        return groups.ToList();
    }

    public async Task<GroupEntity?> GetGroupById(int id)
    {
        var group = await _repository.GetEntityById(id);
        return group;
    }

    public async Task AddNewGroup(GroupEntity entity)
    {
        await _repository.InsertNewEntity(entity);
    }

    public async Task UpdateGroup(GroupEntity entity)
    {
        await _repository.UpdateEntity(entity);
    }

    public async Task DeleteGroup(GroupEntity entity)
    {
        await _repository.DeleteEntity(entity);
    }
}