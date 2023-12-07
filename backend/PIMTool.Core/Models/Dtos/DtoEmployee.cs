namespace PIMTool.Core.Models.Dtos;

public class DtoEmployee
{
    public Guid Id { get; set; }
    
    public string Visa { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    // public DateTime BirthDate { get; set; }
}