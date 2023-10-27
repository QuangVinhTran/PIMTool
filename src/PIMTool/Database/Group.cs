using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Database
{
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Version { get; set; }

        [ForeignKey("GroupLeaderId")]
        public int GroupLeaderId { get; set; }


        public Employee GroupLeader { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
