using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class MissingConnectionStringException : ArgumentNullException, IAppException
{
    public override string Message => "Cannot find connection string";
}