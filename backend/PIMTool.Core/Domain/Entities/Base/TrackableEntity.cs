using System;

namespace PIMTool.Core.Domain.Entities.Base;

public abstract class TrackableEntity<T> : Entity<T>
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}