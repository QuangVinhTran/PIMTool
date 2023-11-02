using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Core.Domain.Entities
{
    public class Employee : IEntity
    {
        public int Id { get; set; }

        [MaxLength(3)]
        public string Visa { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public Group Group { get; set; }

        public ICollection<ProjectEmployee> ProjectEmployees { get; set; }
    }
}
