using System;
using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class MissingJwtSettingsException : MissingMemberException, IAppException
{
    public override string Message => "Cannot find jwt settings";
}