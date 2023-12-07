using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class ProjectDoesNotExistException : ArgumentNullException, IAppException
{
    public override string Message => "Project does not exist";
}