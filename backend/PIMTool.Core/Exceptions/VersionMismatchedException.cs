using System.Data;
using PIMTool.Core.Exceptions.Base;

namespace PIMTool.Core.Exceptions;

public class VersionMismatchedException : VersionNotFoundException, IAppException
{
    public override string Message => "Version is not compatible";
}