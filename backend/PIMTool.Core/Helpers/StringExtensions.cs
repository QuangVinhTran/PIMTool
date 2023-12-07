namespace PIMTool.Core.Helpers;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }
    public static bool IsNotNullNorEmpty(this string value)
    {
        return !string.IsNullOrEmpty(value);
    }
}