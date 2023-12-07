using PIMTool.Core.Domain.Entities.Base;

namespace PIMTool.Core.Domain.Entities;

public class Group : VersionableEntity<Guid, Guid>
{
    public Guid LeaderId { get; set; }
    public string Name { get; set; }
    
    public virtual Employee Leader { get; set; } = null!;
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}