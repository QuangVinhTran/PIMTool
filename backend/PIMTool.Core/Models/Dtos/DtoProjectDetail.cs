namespace PIMTool.Core.Models.Dtos;

public class DtoProjectDetail
{
    public Guid Id { get; set; }
    public int ProjectNumber { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public string Customer { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Version { get; set; }
    public Guid GroupId { get; set; }
    public virtual ICollection<DtoEmployee> Employees { get; set; } = new List<DtoEmployee>();
}