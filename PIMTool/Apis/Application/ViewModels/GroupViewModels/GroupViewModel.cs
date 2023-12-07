namespace Application.ViewModels.GroupViewModels
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public byte[] Version { get; set; }
        public string Name { get; set; }
        public int? GroupLeaderId { get; set; }
    }
}