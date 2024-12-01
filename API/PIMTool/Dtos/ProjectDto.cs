using System.ComponentModel.DataAnnotations;

namespace PIMTool.Dtos;

public class ProjectDto
{
    public int Id { get; set; }
    public int ProjectNumber { get; set; }
    public string Name { get; set; } = null!;
    public string Customer { get; set; }
    public string Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}