using System;
using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class UserAlreadyExistsException : ArgumentException, IAppException
{
    public override string Message => "User already exists";
}