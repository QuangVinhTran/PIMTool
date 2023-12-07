using System.Collections.Generic;
using PIMTool.Core.Models.Request.Base;

namespace PIMTool.Core.Models.Request;

public class SearchEmployeesRequest : BaseSearchRequest
{
    public IList<SortByInfo>? SortByInfos { get; set; }
    public SearchCriteria? SearchCriteria { get; set; }
}