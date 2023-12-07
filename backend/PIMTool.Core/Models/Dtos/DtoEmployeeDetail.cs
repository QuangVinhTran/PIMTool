using PIMTool.Core.Domain.Entities;

namespace PIMTool.Core.Models.Dtos;

public class DtoEmployeeDetail
{
    public string Visa { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime BirthDate { get; set; }
    
    public virtual ICollection<DtoGroup> Groups { get; set; } = new List<DtoGroup>();

    public virtual ICollection<DtoProject> Projects { get; set; } = new List<DtoProject>();
}