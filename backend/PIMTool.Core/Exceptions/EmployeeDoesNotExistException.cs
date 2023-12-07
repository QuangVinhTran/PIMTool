using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class EmployeeDoesNotExistException : ArgumentNullException, IAppException
{
    public override string Message => "Employee does not exist";
}