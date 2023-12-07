using System.Security.Authentication;
using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class InvalidCredentialsException : InvalidCredentialException, IAppException
{
    public override string Message => "Invalid password";
}