namespace PIMTool.Core.Models.Dtos;

public class DtoGroupDetail
{
    public Guid Id { get; set; }
    public Guid LeaderId { get; set; }
    
    public virtual ICollection<DtoProject> Projects { get; set; } = new List<DtoProject>();
}