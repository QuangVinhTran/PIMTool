using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PIMTool.Database
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProjectNumber { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Customer { get; set; }

        [Required, MaxLength(3)]
        public string Status { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public int Version { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }

        public ICollection<ProjectEmployee> ProjectEmployees { get; set; }

    }
}
