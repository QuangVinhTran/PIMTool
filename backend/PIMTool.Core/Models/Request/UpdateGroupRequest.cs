using PIMTool.Core.Attributes;

namespace PIMTool.Core.Models.Request;

public class UpdateGroupRequest
{
    [RequiredGuid]
    public Guid LeaderId { get; set; }

    public string? Name { get; set; }
}