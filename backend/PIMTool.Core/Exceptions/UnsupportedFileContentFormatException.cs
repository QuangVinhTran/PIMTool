using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class UnsupportedFileContentFormatException : ArgumentException, IAppException
{
    public override string Message => "File content format is not supported";
}