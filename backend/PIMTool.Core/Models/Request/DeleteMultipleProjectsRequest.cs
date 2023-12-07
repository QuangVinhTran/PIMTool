namespace PIMTool.Core.Models.Request;

public class DeleteMultipleProjectsRequest
{
    public IList<Guid> ProjectIds { get; set; }
}