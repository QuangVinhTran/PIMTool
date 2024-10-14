namespace PIMTool.Payload.Request.Service;

public class CEmployeeRequest
{
    public string Visa { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public decimal Version { get; set; }

    public override string ToString()
    {
        return base.ToString();
    }
}