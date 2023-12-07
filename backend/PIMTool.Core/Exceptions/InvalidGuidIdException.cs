using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class InvalidGuidIdException : ArgumentException, IAppException
{
    public override string Message => "Invalid guid id";
}