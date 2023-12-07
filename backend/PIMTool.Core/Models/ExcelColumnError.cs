namespace PIMTool.Core.Models;

public class ExcelColumnError
{
    public short ColumnNumber { get; set; }
    public string ErrorDetail { get; set; } = string.Empty;

    public ExcelColumnError()
    {
    }

    public ExcelColumnError(short columnNumber, string errorDetail)
    {
        ColumnNumber = columnNumber;
        ErrorDetail = errorDetail;
    }
}