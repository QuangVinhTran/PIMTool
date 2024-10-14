namespace PIMTool.Payload.Response;

public class EmployeeResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Visa { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public decimal Version { get; set; }
}