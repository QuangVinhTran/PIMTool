namespace PIMTool.Core.Models;

public class SearchCriteria
{
    public IList<SearchByInfo> ConjunctionSearchInfos { get; set; }
    public IList<SearchByInfo> DisjunctionSearchInfos { get; set; }

}