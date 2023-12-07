namespace PIMTool.Payload.Response;

public class BaseResponse
{
    public string Message { get; set; } = null!;
    public object Data { get; set; } = null!;

    public BaseResponse()
    {
    }

    public BaseResponse(string message, object data)
    {
        Message = message;
        Data = data;
    }
}