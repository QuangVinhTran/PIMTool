using System.ComponentModel.DataAnnotations;

namespace PIMTool.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Visa { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
    }
}
