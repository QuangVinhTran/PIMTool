using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class RefreshTokenDoesNotExistException : ArgumentNullException, IAppException
{
    public override string Message => "Refresh token does not exist";
}