using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMTool.Core.Domain.Entities;

[Index("ProjectNumber", IsUnique = true)]
public class Project : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int GroupId { get; set; }

   // [Required, Range(0, 999)]
    public int ProjectNumber { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(50)]
    public string Customer { get; set; }

    [Required, MaxLength(3)]
    public string Status { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Timestamp]
    public byte[]? Version { get; set; }

    [ForeignKey("GroupId")]
    public Group Group { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}