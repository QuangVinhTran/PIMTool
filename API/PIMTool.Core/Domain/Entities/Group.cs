using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Core.Domain.Entities
{
    public class Group : IEntity
    {
        [Required]
        public int Id { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public int GroupLeaderId { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
