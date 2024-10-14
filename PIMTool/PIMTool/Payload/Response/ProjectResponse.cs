namespace PIMTool.Payload.Response;

public class ProjectResponse
{
    public int Id { get; set; }
    public int ProjectNumber { get; set; }
    public string Name { get; set; } = null!;
    public string Customer { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = null!;
    public int GroupId { get; set; }
    public decimal Version { get; set; } 

    public ProjectResponse()
    {
    }
}