using System.ComponentModel.DataAnnotations;

namespace PIMTool.Dtos
{
    public class CreateEmployeeRequestDto
    {
        public string Visa { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
