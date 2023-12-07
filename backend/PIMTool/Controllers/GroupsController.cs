using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Controllers.Base;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Core.Models.Request;

namespace PIMTool.Controllers;

[Authorize]
[Route("[controller]")]
public class GroupsController : BaseController
{
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [Route("all")]
    [HttpGet]
    public async Task<IActionResult> GetAllGroups()
    {
        return await ExecuteApiAsync(
            async () => await _groupService.GetAllGroupsAsync().ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    
    [HttpPost]
    public async Task<IActionResult> GetGroups(SearchGroupsRequest searchGroupsRequest)
    {
        return await ExecuteApiAsync(
            async () => await _groupService.FindGroupsAsync(searchGroupsRequest).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    
    [Route("{id:guid}")]
    [HttpPost]
    public async Task<IActionResult> GetGroup(Guid id)
    {
        return await ExecuteApiAsync(
            async () => await _groupService.FindGroupAsync(id).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    
    [Route("create")]
    [HttpPost]
    public async Task<IActionResult> CreateGroup(CreateGroupRequest createGroupRequest)
    {
        return await ExecuteApiAsync(
            async () => await _groupService.CreateGroupAsync(createGroupRequest).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    
    [Route("update/{id:guid}")]
    [HttpPut]
    public async Task<IActionResult> UpdateGroup(UpdateGroupRequest updateGroupRequest, Guid id)
    {
        var updaterId = HttpContext.Request.Headers["UpdaterId"].ToString();
        return await ExecuteApiAsync(
            async () => await _groupService.UpdateGroupAsync(updateGroupRequest, id, updaterId).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    
    [Route("delete/{id:guid}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteGroup(Guid id)
    {
        return await ExecuteApiAsync(
            async () => await _groupService.DeleteGroupAsync(id).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
}