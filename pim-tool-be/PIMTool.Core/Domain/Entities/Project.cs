using PIMTool.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PIMTool.Core.Domain.Entities
{
    public class Project : IEntity
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        public int ProjectNumber { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Customer { get; set; }

        [MaxLength(3)]
        public Status Status { get; set; }

        public DateTime StartDate { get; set; }
       
        public DateTime? EndDate { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public int GroupId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Group Group { get; set; }

        public ICollection<ProjectEmployee> ProjectEmployees { get; set; }

    }
}