using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.ViewModels.ProjectViewModels
{
    public class UpdateProjectViewModel
    {
        // public byte[] Version { get; set; }

        [Required]
        [Range(1000, 9999)]
        public int ProjectNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Customer { get; set; }

        [Required]
        public StatusEnum Status { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int GroupId { get; set; }
        public List<int>? SelectedEmployeeId { get; set; }
    }
}