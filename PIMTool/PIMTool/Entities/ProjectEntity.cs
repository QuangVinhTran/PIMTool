using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PIMTool.Entities;

[Table("Projects")]
public class ProjectEntity : BaseEntity
{
    [Required]
    public int ProjectNumber { get; set; }

    [Required] [StringLength(50)] public string Name { get; set; } = null!;

    [Required] [StringLength(50)] public string Customer { get; set; } = null!;
    
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }

    [Required] [StringLength(3)] public string Status { get; set; } = null!;
    public int GroupId { get; set; }
    [ForeignKey("GroupId")] public virtual GroupEntity Group { get; set; } = null!;
    [ConcurrencyCheck] public decimal Version { get; set; } 

}
