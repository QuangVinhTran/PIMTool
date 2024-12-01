using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMTool.Core.Domain.Entities
{
    public class ProjectEmployee : IEntity
    {
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public Project Project { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
        [NotMapped]
        [JsonIgnore]
        public int Id { get; set; }
    }
}
