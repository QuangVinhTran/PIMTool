namespace Application.ViewModels.GroupViewModels
{
    public class CreateGroupViewModel
    {
        public int Id;
        public string Name { get; set; }
        public int? GroupLeaderId { get; set; }
    }
}