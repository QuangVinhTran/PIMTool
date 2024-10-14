using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Entities;
using PIMTool.Payload.Request.Paging;
using PIMTool.Payload.Request.Service;
using PIMTool.Payload.Response;
using PIMTool.Services;

namespace PIMTool.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IGroupService _groupService;

    public GroupController(IMapper mapper, IGroupService groupService)
    {
        _mapper = mapper;
        _groupService = groupService;
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllGroups()
    {
        var groups = await _groupService.GetAllGroups();
        return (!groups.Any())
            ? NotFound(new BaseResponse("Not Found!", null))
            : Ok(new BaseResponse("Successful!", _mapper.Map<List<GroupResponse>>(groups)));
    }
    
    [HttpGet("get-all-with-paging/{currentPage}/{pageSize}")]
    public async Task<IActionResult> GetAllGroupsWithPaging(int currentPage, int pageSize)
    {
        var groups = await _groupService.GetAllGroupsWithPaging(new PagingParameter(currentPage, pageSize));
        return (!groups.Any())
            ? NotFound(new BaseResponse("Not Found!", null))
            : Ok(new BaseResponse("Successful!", groups));
    }

    [HttpPost("insert")]
    public async Task<IActionResult> InsertNewGroup(CGroupRequest request)
    {
        try
        {
            await _groupService.AddNewGroup(_mapper.Map<CGroupRequest, GroupEntity>(request));
            return Created("", new BaseResponse("Insert new group successful!", null));
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse("Insert new group unsuccessful!", ex.Message));
        }
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteEmployee(DGroupRequest request)
    {
        await _groupService.DeleteGroup(_mapper.Map<DGroupRequest, GroupEntity>(request));
        return Ok(new BaseResponse("Delete successfully!", null));
    }
    
}