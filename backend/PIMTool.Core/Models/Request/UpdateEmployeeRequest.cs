namespace PIMTool.Core.Models.Request;

public class UpdateEmployeeRequest
{
    public string Visa { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime BirthDate { get; set; }
}