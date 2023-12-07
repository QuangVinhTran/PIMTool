namespace PIMTool.Payload.Request.Service;

public class CProjectRequest
{
    public int ProjectNumber { get; set; }
    public string Name { get; set; } = null!;
    public string Customer { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int GroupId { get; set; }
}
