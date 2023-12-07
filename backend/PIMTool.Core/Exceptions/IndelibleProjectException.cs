using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class IndelibleProjectException : OperationCanceledException, IAppException
{
    public override string Message => "Cannot delete non-new project";
}