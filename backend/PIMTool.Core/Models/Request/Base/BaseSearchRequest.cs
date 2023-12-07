using System.ComponentModel.DataAnnotations;

namespace PIMTool.Core.Models.Request.Base;

public class BaseSearchRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int PageIndex { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int PageSize { get; set; }
}