namespace PIMTool.Core.Models;

public class ApiActionResult
{
    
    public bool IsSuccess { get; set; }
    public object Data { get; set; }
    public string Detail { get; set; }

    public ApiActionResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }
    
    public ApiActionResult(bool isSuccess, string detail)
    {
        IsSuccess = isSuccess;
        Detail = detail;
    }

    public ApiActionResult() : this(true)
    {
    }
}