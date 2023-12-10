using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace PIMTool.Core.Domain.Entities
{
    public class Project : IEntity
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ProjectNumber { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        [Required]
        public string Customer { get; set; }

        [MaxLength(3)]
        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Timestamp]
        [ConcurrencyCheck]
        public byte[] Version { get; set; }

        public int GroupId { get; set; }
        public Group? Group { get; set; }
        public ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();
    }
}