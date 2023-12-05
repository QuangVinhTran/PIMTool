namespace PIMTool.Dtos
{
    public class ProjectEmployeeDto
    {
        public int ProjectId { get; set; }
        public ProjectDto Project { get; set; }

        public int EmployeeId { get; set; }
        public EmployeeDto Employee { get; set; }
    }
}
