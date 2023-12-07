using PIMTool.Core.Interfaces.Services.Base;
using PIMTool.Core.Models;
using PIMTool.Core.Models.Request;

namespace PIMTool.Core.Interfaces.Services;

public interface IGroupService : IService
{
    Task<ApiActionResult> GetAllGroupsAsync();
    Task<ApiActionResult> CreateGroupAsync(CreateGroupRequest createGroupRequest);
    Task<ApiActionResult> FindGroupsAsync(SearchGroupsRequest searchGroupsRequest);
    Task<ApiActionResult> FindGroupAsync(Guid id);
    Task<ApiActionResult> UpdateGroupAsync(UpdateGroupRequest request, Guid id, string updaterId);
    Task<ApiActionResult> DeleteGroupAsync(Guid id);
}