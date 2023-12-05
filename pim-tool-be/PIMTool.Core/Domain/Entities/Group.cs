using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PIMTool.Core.Domain.Entities
{
    public class Group : IEntity
    {
        public int Id { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public int GroupLeaderId { get; set; }

        [JsonIgnore]
        public Employee GroupLeader { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
