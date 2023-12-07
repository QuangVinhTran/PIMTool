using PIMTool.Core.Domain.Entities.Base;

namespace PIMTool.Core.Domain.Entities;

public class Project : VersionableEntity<Guid, Guid>
{
    public Guid GroupId { get; set; }
    public int ProjectNumber { get; set; }
    public string Name { get; set; } = null!;
    public string Customer { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public virtual Group Group { get; set; } = null!;
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}