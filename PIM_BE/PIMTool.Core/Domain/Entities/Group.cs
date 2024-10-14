using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMTool.Core.Domain.Entities;

[Index("GroupLeaderId", IsUnique = true)]
public class Group : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get ; set; }
    public int? GroupLeaderId { get; set; }
    [Timestamp]
    public byte[] Version { get; set; }

    [ForeignKey("GroupLeaderId")]
    public Employee Employee { get; set; }
}
