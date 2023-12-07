using PIMTool.Core.Models.Request.Base;

namespace PIMTool.Core.Models.Request;

public class SearchGroupsRequest : BaseSearchRequest
{
    public IList<SortByInfo>? SortByInfos { get; set; }
    public SearchCriteria? SearchCriteria { get; set; }
}