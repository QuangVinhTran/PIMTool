using PIMTool.Core.Enums;

namespace PIMTool.Core.Models;

public class AppMessage
{
    public string Content { get; set; }
    public AppMessageType Type { get; set; }
}