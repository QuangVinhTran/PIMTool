using System.Collections.Generic;
using PIMTool.Core.Enums;

namespace PIMTool.Core.Models;


public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public IList<AppMessage> Messages { get; set; }
    public ApiResponse(bool isSuccess)
    {
        IsSuccess = isSuccess;
        Messages = new List<AppMessage>();
    }

    public ApiResponse() : this(false)
    {
    }

    public ApiResponse<T> AddMessage(string messageContent, AppMessageType type)
    {
        Messages.Add(new AppMessage() {Content = messageContent, Type = type});
        return this;
    }

    public ApiResponse<T> AddSuccessMessage(string messageContent)
    {
        Messages.Add(new AppMessage(){ Content = messageContent, Type = AppMessageType.Success});
        return this;
    }
    
    public ApiResponse<T> AddWarningMessage(string messageContent)
    {
        Messages.Add(new AppMessage(){ Content = messageContent, Type = AppMessageType.Warning});
        return this;
    }
    
    public ApiResponse<T> AddErrorMessage(string messageContent)
    {
        Messages.Add(new AppMessage(){ Content = messageContent, Type = AppMessageType.Error});
        return this;
    }
}

public class ApiResponse : ApiResponse<object>
{
    public ApiResponse(bool isSuccess): base(isSuccess)
    {
    }

    public ApiResponse() : this(false)
    {
    }

    public ApiResponse(object data) : this()
    {
        Data = data;
    }
}