using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Core.Domain.Entities
{
    public class Group : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int GroupLeaderId { get; set; }
        public string Name { get; set; }
        [Timestamp]
        public byte[]? Version { get; set; }

        public Employee GroupLeader { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
