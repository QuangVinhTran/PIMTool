namespace PIMTool.Core.Models.Settings;

public class JwtSettings
{
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SigningKey { get; init; }
}