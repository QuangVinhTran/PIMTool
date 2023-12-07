namespace PIMTool.Core.Helpers;

public static class ExcelFileHelper
{
    private static readonly string[] EXCEL_FILE_EXTENSIONS = {".xlsx", ".xlsb", ".xls"};
    private static readonly string[] VALID_HEADERS = {"PROJECT NUMBER", "PROJECT NAME", "CUSTOMER", "GROUP", "MEMBERS", "STATUS", "START DATE", "END DATE"};

    public static bool IsExcelFile(this string fileName)
    {
        return EXCEL_FILE_EXTENSIONS.Contains(Path.GetExtension(fileName).ToLowerInvariant());
    }

    public static bool IsCsvFile(this string fileName)
    {
        return ".csv".Equals(Path.GetExtension(fileName).ToLowerInvariant());
    }

    public static bool ValidHeaders(object?[] headers)
    {
        if (headers.Length != VALID_HEADERS.Length)
        {
            return false;
        }

        foreach (var header in headers)
        {
            var headerString = header?.ToString() ?? string.Empty;
            if (headerString.IsNullOrEmpty())
            {
                return false;
            }

            if (!VALID_HEADERS.Contains(headerString.Trim().ToUpper()))
            {
                return false;
            }
        }

        return true;
    }
}