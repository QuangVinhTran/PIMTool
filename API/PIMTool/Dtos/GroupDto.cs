using PIMTool.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Dtos
{
    public class GroupDto
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public int GroupLeaderId { get; set; }
        public ICollection<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
    }
}
