using System.ComponentModel.DataAnnotations;

namespace PIMTool.Core.Models;

public class SortByInfo
{
    [Required] 
    public string FieldName { get; set; }
    public bool Ascending { get; set; }

    public SortByInfo()
    {
    }
    
    public SortByInfo(string fieldName)
    {
        FieldName = fieldName;
        Ascending = true;
    }

    public SortByInfo(string fieldName, bool ascending)
    {
        FieldName = fieldName;
        Ascending = ascending;
    }
}