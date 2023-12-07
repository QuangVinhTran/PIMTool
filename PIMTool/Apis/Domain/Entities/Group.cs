using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Group : IEntity
{
    public int Id { get; set; }
    [Timestamp]
    public byte[] Version { get; set; }
    public string Name { get; set; }
    public int? GroupLeaderId { get; set; }

    public virtual Employee GroupLeader { get; set; }

    public virtual ICollection<Project> Projects { get; set; }
}
