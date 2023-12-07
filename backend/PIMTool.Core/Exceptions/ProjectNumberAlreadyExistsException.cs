using System;
using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class ProjectNumberAlreadyExistsException : ArgumentException, IAppException
{
    public override string Message => "Project number already exists";
}