using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMTool.Entities;

[Table("ProjectEmployee")]
public class ProjectEmployeeEntity
{
    [ForeignKey("ProjectId")]
    public int ProjectId { get; set; }
    public virtual ProjectEntity Project { get; set; } = null!;
    
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }
    public virtual EmployeeEntity Employee { get; set; } = null!;
    
    public ProjectEmployeeEntity()
    {
    }
}