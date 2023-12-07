namespace PIMTool.Core.Models;

public class PaginatedResult
{
    public long PageIndex { get; set; }
    public int PageSize { get; set; }
    public long LastPage { get; set; }
    public bool IsLastPage { get; set; }
    public long Total { get; set; }
    public object Data { get; set; }
}