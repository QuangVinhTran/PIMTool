using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMTool.Entities;

[Table("Users")]
public class UserEntity : BaseEntity
{
    [Required]
    [StringLength(32)]
    public string Username { get; set; } = null!;
    [Required] 
    [StringLength(32)] 
    public string Password { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }

    public UserEntity(string username)
    {
        Username = username;
    }

    public UserEntity()
    {
    }

    protected UserEntity(string username, string password, string refreshToken, DateTime tokenCreated, DateTime tokenExpires)
    {
        Username = username;
        Password = password;
        RefreshToken = refreshToken;
        TokenCreated = tokenCreated;
        TokenExpires = tokenExpires;
    }
}