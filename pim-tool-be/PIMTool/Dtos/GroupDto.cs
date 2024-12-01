using PIMTool.Core.Domain.Entities;

namespace PIMTool.Dtos
{
    public class GroupDto
    {
        public int Id { get; set; }
        public byte[] Version { get; set; }
        public int GroupLeaderId { get; set; }
        public EmployeeDto GroupLeader { get; set; }
    }
}
