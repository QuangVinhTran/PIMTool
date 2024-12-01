namespace PIMTool.Dtos
{
    public class GroupDto
    {
        public int Id { get; set; }
        public int GroupLeaderId { get; set; }
        public string Name { get; set; }
        public byte[] Version { get; set; }
    }
}
