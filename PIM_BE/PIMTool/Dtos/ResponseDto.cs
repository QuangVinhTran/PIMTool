namespace PIMTool.Dtos;

public class ResponseDto
{
    public object? Data { get; set; }
    public bool isSuccess { get; set; } = true;
    public string Error { get; set; } = "";
}
