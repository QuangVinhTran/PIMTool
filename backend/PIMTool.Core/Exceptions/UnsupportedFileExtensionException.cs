using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class UnsupportedFileExtensionException : ArgumentException, IAppException
{
    public override string Message => "File extension is not supported";
}