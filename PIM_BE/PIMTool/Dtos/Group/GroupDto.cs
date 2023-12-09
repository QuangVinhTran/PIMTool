using System.ComponentModel.DataAnnotations;

namespace PIMTool.Dtos.Group
{
    public class GroupDto
    {
        [Required]
        public int GroupLeaderId { get; set; }
    }
}
