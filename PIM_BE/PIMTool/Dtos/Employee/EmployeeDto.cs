using PIMTool.Dtos.Project;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Dtos.Employee;

public class EmployeeDto
{
    public int Id { get; set; }

    [Required, MaxLength(3)]
    public string Visa { get; set; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; }

    [Required, MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    public DateTime Birthday { get; set; }
}
