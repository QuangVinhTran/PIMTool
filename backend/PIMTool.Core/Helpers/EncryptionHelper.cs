namespace PIMTool.Core.Helpers;

public static class EncryptionHelper
{
    public static string Encrypt(string rawValue)
    {
        return BCrypt.Net.BCrypt.HashPassword(rawValue);
    }

    public static bool Verify(string rawValue, string encryptedValue)
    {
        return BCrypt.Net.BCrypt.Verify(rawValue, encryptedValue);
    }
}