using System;
using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class GroupDoesNotExistException : ArgumentNullException, IAppException
{
    public override string Message => "Group does not exist";
}