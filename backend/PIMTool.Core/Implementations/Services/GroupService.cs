using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Exceptions;
using PIMTool.Core.Helpers;
using PIMTool.Core.Implementations.Services.Base;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Core.Models;
using PIMTool.Core.Models.Dtos;
using PIMTool.Core.Models.Request;

namespace PIMTool.Core.Implementations.Services;

public class GroupService : BaseService, IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IPIMUserRepository _userRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public GroupService(ILifetimeScope scope) : base(scope)
    {
        _groupRepository = Resolve<IGroupRepository>();
        _userRepository = Resolve<IPIMUserRepository>();
        _employeeRepository = Resolve<IEmployeeRepository>();
        _unitOfWork = Resolve<IUnitOfWork>();
        _mapper = Resolve<IMapper>();
    }

    public async Task<ApiActionResult> GetAllGroupsAsync()
    {
        var groups = await _groupRepository.FindByAsync(g => !g.IsDeleted);
        var groupDtos = _mapper.Map<IEnumerable<DtoGroup>>(await groups.ToListAsync());
        return new ApiActionResult(true) { Data = groupDtos };
    }

    public async Task<ApiActionResult> CreateGroupAsync(CreateGroupRequest createGroupRequest)
    {
        var group = _mapper.Map<Group>(createGroupRequest);
        group.SetCreatedInfo(Guid.Empty);
        await _groupRepository.AddAsync(group);
        await _unitOfWork.CommitAsync();
        return new ApiActionResult(true);
    }

    public async Task<ApiActionResult> FindGroupsAsync(SearchGroupsRequest req)
    {
        var groups = await _groupRepository.FindByAsync(g => !g.IsDeleted);
        if (req.SearchCriteria is not null)
        {
            var conjunctionWhere =
                ExpressionHelper.CombineOrExpressions<Group>(req.SearchCriteria.ConjunctionSearchInfos, p => !p.IsDeleted);
            groups = groups.AsEnumerable().Where(conjunctionWhere.Compile()).AsQueryable();

            var disjunctionWhere =
                ExpressionHelper.CombineAndExpressions<Group>(req.SearchCriteria.DisjunctionSearchInfos, p => true);
            groups = groups.AsEnumerable().Where(disjunctionWhere).AsQueryable();
        }

        var orderedGroups = groups.OrderBy(g => g.CreatedAt);
        if (req.SortByInfos is not null)
        {
            orderedGroups = req.SortByInfos.Aggregate(orderedGroups, (current, sort) => sort.Ascending
                ? current.ThenBy(p => ReflectionHelper.GetPropertyValueByName(p, sort.FieldName))
                : current.ThenByDescending(p => ReflectionHelper.GetPropertyValueByName(p, sort.FieldName)));
        }
        
        var paginatedResult = PaginationHelper.BuildPaginatedResult<Group, DtoGroup>(_mapper, orderedGroups, req.PageSize,
                req.PageIndex);
        return new ApiActionResult(true) { Data = paginatedResult };
    }

    public async Task<ApiActionResult> FindGroupAsync(Guid id)
    {
        var groups = await _groupRepository.FindByAsync(g => g.Id == id && !g.IsDeleted);
        var group = await groups
            .Include(g => g.Projects)
            .FirstOrDefaultAsync();
        if (group is null)
        {
            throw new GroupDoesNotExistException();
        }

        var groupDto = _mapper.Map<DtoGroupDetail>(group);
        return new ApiActionResult(true) { Data = groupDto };
    }

    public async Task<ApiActionResult> UpdateGroupAsync(UpdateGroupRequest request, Guid id, string updaterId)
    {
        var parseSuccess = Guid.TryParse(updaterId, out var updaterGuidId);
        if (!parseSuccess)
        {
            throw new InvalidGuidIdException();
        }
        
        if (!await _userRepository.ExistsAsync(u => !u.IsDeleted && u.Id == updaterGuidId))
        {
            throw new UserDoesNotExistException();
        }

        if (!await _employeeRepository.ExistsAsync(g => !g.IsDeleted && g.Id == request.LeaderId))
        {
            throw new EmployeeDoesNotExistException();
        }

        var group = await (await _groupRepository
                .FindByAsync(e => !e.IsDeleted && e.Id == id))
            .FirstOrDefaultAsync();
        if (group is null)
        {
            throw new GroupDoesNotExistException();
        }
        _mapper.Map(request, group);
        group.SetUpdatedInfo(updaterGuidId);

        await _groupRepository.UpdateAsync(group);
        await _unitOfWork.CommitAsync();
        
        return new ApiActionResult(true);
    }

    public async Task<ApiActionResult> DeleteGroupAsync(Guid id)
    {
        if (!await _groupRepository.ExistsAsync(e => !e.IsDeleted && e.Id == id))
        {
            throw new GroupDoesNotExistException();
        }

        await _groupRepository.DeleteAsync(id);
        await _unitOfWork.CommitAsync();
        return new ApiActionResult(true);
    }
}