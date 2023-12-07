namespace PIMTool.Payload.Response;

public class GroupResponse
{
    public int Id { get; set; }
    public EmployeeResponse GroupLeader { get; set; } = null!;
    public decimal Version { get; set; } 
}