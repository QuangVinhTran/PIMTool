using System;

namespace PIMTool.Core.Domain.Entities.Base;

public abstract class CreatableEntity<TKey, TUserKey> : Entity<TKey> 
{
    public DateTime CreatedAt { get; set; }
    public TUserKey CreatedBy { get; set; }
    // public bool IsDeleted { get; set; }

    public virtual void SetCreatedInfo(TUserKey createdBy)
    {
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }
}