using System;

namespace PIMTool.Core.Domain.Entities.Base;

public abstract class AuditableEntity<TKey, TUserKey> : CreatableEntity<TKey, TUserKey>
{
    public DateTime UpdatedAt { get; set; }
    public TUserKey UpdatedBy { get; set; }

    public virtual void SetUpdatedInfo(TUserKey updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.Now;
    }

    public override void SetCreatedInfo(TUserKey createdBy)
    {
        base.SetCreatedInfo(createdBy);
        SetUpdatedInfo(createdBy);
    }
}