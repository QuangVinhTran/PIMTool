using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMTool.Entities;

[Table("Groups")]
public class GroupEntity : BaseEntity
{
    public int GroupLeaderId { get; set; }
    [ForeignKey("GroupLeaderId")]
    public virtual EmployeeEntity GroupLeader { get; set; } = null!;

    [ConcurrencyCheck] public decimal Version { get; set; } 

}