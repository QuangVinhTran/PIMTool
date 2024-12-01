namespace Domain.Entities;
public class ProjectEmployee
{
    public int ProjectId { get; set; }

    public int EmployeeId { get; set; }

    public virtual Project Project { get; set; }

    public virtual Employee Employee { get; set; }
}