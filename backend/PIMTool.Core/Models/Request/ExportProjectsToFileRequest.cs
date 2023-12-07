using PIMTool.Core.Attributes;

namespace PIMTool.Core.Models.Request;

public class ExportProjectsToFileRequest
{
    public string ProjectName { get; set; }
    public string Customer { get; set; }
    public string LeaderName { get; set; }
    [DateIsBefore(nameof(StartDateTo), "")]
    public DateTime StartDateFrom { get; set; }
    public DateTime StartDateTo { get; set; }
    [DateIsBefore(nameof(EndDateTo), "")]
    public DateTime EndDateFrom { get; set; }
    public DateTime EndDateTo { get; set; }
    public string Status { get; set; }
    public string OrderBy { get; set; }
    public int NumberOfRows { get; set; }
}