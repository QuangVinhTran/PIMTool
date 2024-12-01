namespace PIMTool.Dtos
{
    public class SearchResultResponse
    {
        public int totalCount { get; set; }
        public IEnumerable<ProjectDto> results { get; set; }
    }
}
