using Newtonsoft.Json;
using PIMTool.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMTool.Dtos;

public class ProjectDto
{
    public int Id { get; set; }

    public int GroupId { get; set; }
    public int ProjectNumber { get; set; }
    public string Name { get; set; } = null!;
    public string Customer { get; set; } = null!;
    // Status value should be 3 letters: NEW(New), PLA(Planned), INP(In progress), FIN(Finished)
    public string Status { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public byte[]? Version { get; set; }
    public ICollection<Employee>? Employees { get; set; }

    [NotMapped]
    public List<string>? EmployeeVisas { get; set; }
}