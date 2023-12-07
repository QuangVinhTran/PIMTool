namespace PIMTool.Payload.Request.Authentication;

public class UserAuthentication
{
    public string Username { get; set; }
    public string Password { get; set; }
    public UserAuthentication(string username, string password)
    {
        Username = username;
        Password = password;
    }
}