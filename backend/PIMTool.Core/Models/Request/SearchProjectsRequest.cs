using System.Collections.Generic;
using PIMTool.Core.Models.Request.Base;

namespace PIMTool.Core.Models.Request;

public class SearchProjectsRequest : BaseSearchRequest
{
    public IList<SortByInfo>? SortByInfos { get; set; }
    public SearchCriteria? SearchCriteria { get; set; }
    public AdvancedFilter? AdvancedFilter { get; set; }
}