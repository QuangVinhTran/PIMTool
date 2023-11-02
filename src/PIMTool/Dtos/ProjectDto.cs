using PIMTool.Core.Domain.Enums;

namespace PIMTool.Dtos;

public class ProjectDto
{
    public int Id { get; set; }
    public int ProjectNumber { get; set; }
    public string Name { get; set; }
    public string Customer { get; set; }
    public Status Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public byte[] Version { get; set; }
    public int GroupId { get; set; }
    public GroupDto Group { get; set; }
}