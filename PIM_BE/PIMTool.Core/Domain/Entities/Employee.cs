using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMTool.Core.Domain.Entities;

public class Employee : IEntity
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(3)]
    public string Visa { get; set; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; }

    [Required, MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    public DateTime Birthday { get;set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    [Timestamp]
    public byte[] Version { get ; set; }

}
