using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Employee : IEntity
{
    public int Id { get; set; }

    [Timestamp]
    public byte[]? Version { get; set; }

    [Required]
    [MaxLength(3)]
    public string Visa { get; set; }
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }
    public Group Group { get; set; }

    public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; }
}
