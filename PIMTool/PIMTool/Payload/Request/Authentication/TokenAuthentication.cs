namespace PIMTool.Payload.Request.Authentication;

public class TokenAuthentication
{
    public string Token { get; set; }
    public RefreshToken RefreshToken { get; set; }

    public TokenAuthentication(string token, RefreshToken refreshToken)
    {
        Token = token;
        RefreshToken = refreshToken;
    }
}