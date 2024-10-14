using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMTool.Entities;

[Table("Employees")]
public class EmployeeEntity : BaseEntity
{
    [Required] [StringLength(3)] public string Visa { get; set; } = null!;

    [Required] [StringLength(50)] public string FirstName { get; set; } = null!;
    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = null!;
    
    [Required]
    public DateTime BirthDate { get; set; }

    [ConcurrencyCheck] public decimal Version { get; set; } 

}