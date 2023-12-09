using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.ViewModels.ProjectViewModels
{
    public class CreateProjectViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ProjectNumber is required")]
        [Range(1000, 9999, ErrorMessage = "ProjectNumber must be between 1000 and 9999")]
        public int ProjectNumber { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        [MaxLength(50, ErrorMessage = "Customer cannot exceed 50 characters")]
        public string Customer { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public StatusEnum Status { get; set; }

        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int GroupId { get; set; }
        public List<int>? SelectedEmployeeId { get; set; }

        public bool IsEndDateValid()
        {
            if (EndDate.HasValue) // If EndDay not null
            {
                return EndDate.Value > StartDate; // if EndDay > StartDate => true
            }
            return true; // EndDay null => true
        }
    }
}