namespace PIMTool.Payload.Request.Authentication;

public class RefreshToken
{
    public string Token { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Expired { get; set; }
}