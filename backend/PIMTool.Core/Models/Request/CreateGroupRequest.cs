using System.ComponentModel.DataAnnotations;
using PIMTool.Core.Attributes;

namespace PIMTool.Core.Models.Request;

public class CreateGroupRequest
{
    [RequiredGuid] 
    public Guid LeaderId { get; set; }

    [Required]
    public string Name { get; set; }
}