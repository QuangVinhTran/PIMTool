namespace PIMTool.Payload.Request.Paging;

public class PagingParameter
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }

    public PagingParameter(int currentPage, int pageSize)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
    }
}