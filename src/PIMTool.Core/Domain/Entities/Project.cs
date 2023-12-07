using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace PIMTool.Core.Domain.Entities
{
    public class Project : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int ProjectNumber { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Customer { get; set; } = null!;
        [Required,StringLength(3)]
        public ProjectStatus Status { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Timestamp]
        public byte[]? Version { get; set; }
        [JsonIgnore]
        public Group? Group { get; set; }
        [JsonIgnore]
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        [JsonIgnore]
        public ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();
    }

    public enum ProjectStatus
    {
        NEW,
        PLA, //Planned
        INP, //In progress
        FIN //Finished
    }
}
