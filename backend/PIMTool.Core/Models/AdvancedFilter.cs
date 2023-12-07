namespace PIMTool.Core.Models;

public class AdvancedFilter
{
    public string LeaderName { get; set; }
    public string MemberName { get; set; }
    public DateRange? StartDateRange { get; set; }
    public DateRange? EndDateRange { get; set; }
}