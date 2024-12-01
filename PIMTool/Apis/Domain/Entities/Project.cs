using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;
public class Project : IEntity
{
    public int Id { get; set; }
    [Timestamp]
    public byte[] Version { get; set; }
    [Required]
    [Range(1000, 9999)]
    public int ProjectNumber { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    [Required]
    [MaxLength(50)]
    public string Customer { get; set; }
    [Required]
    public StatusEnum Status { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int GroupId { get; set; }
    public virtual Group Group { get; set; }
    public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; }

}
