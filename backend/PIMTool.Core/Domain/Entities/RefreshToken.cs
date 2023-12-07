using System;
using PIMTool.Core.Domain.Entities.Base;

namespace PIMTool.Core.Domain.Entities;

public class RefreshToken : TrackableEntity<Guid>
{
    public Guid Token { get; set; }
    public Guid UserId { get; set; }
    public string IPAddress { get; set; }
    public DateTime ValidUntil { get; set; }
    
    public virtual PIMUser? User { get; set; }
}