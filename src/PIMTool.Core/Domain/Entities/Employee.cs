using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Core.Domain.Entities
{
    public class Employee : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(3)]
        public string Visa { get; set; }
        [Required,StringLength(50)]
        public string FirstName { get; set; }
        [Required, StringLength(50)]
        public string LastName { get; set; } 
        [Required]
        public DateTime BirthDate { get; set; }
        [Timestamp]
        public byte[]? Version { get; set; }
        [JsonIgnore]
        public Group? Group { get; }
        [JsonIgnore]
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        [JsonIgnore]
        public ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();
    }
}
