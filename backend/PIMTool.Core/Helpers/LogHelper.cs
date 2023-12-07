using PIMTool.Core.Interfaces.Services;

namespace PIMTool.Core.Helpers;

public class LogHelper
{
    private static ILoggerService _loggerservice;

    public static void InitLoggerService(ILoggerService loggerService)
    {
        _loggerservice = loggerService;
    }

    public static void WriteInfo(string message)
    {
        _loggerservice.Info(message);
    }
}