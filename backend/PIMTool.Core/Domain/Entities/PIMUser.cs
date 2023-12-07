using System;
using System.Collections.Generic;
using PIMTool.Core.Domain.Entities.Base;

namespace PIMTool.Core.Domain.Entities;

public class PIMUser : VersionableEntity<Guid, Guid>
{
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
            
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}