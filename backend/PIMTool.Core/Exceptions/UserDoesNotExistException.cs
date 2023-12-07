using System;
using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class UserDoesNotExistException : ArgumentNullException, IAppException
{
    public override string Message => "User does not exist";
}