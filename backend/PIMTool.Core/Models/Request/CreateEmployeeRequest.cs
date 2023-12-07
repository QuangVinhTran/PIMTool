using System.ComponentModel.DataAnnotations;
using PIMTool.Core.Attributes;

namespace PIMTool.Core.Models.Request;

public class CreateEmployeeRequest
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