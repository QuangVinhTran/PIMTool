namespace PIMTool.Dtos
{
    public class CreateProjectRequestDto
    {
        public int ProjectNumber { get; set; }
        public string Name { get; set; }
        public string Customer { get; set; }
        public string Status { get; set; }
        public int GroupId { get; set; }
        public List<int> Members { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
