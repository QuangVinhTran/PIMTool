using System.ComponentModel.DataAnnotations;
using PIMTool.Core.Attributes;

namespace PIMTool.Core.Models;

public class EmployeeCreateModel
{
    [Required]
    public string Visa { get; set; } = null!;

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [RequiredDate]
    public DateTime BirthDate { get; set; }
}