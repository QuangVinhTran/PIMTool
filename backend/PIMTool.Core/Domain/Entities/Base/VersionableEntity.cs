namespace PIMTool.Core.Domain.Entities.Base;

public abstract class VersionableEntity<TKey, TUserKey> : AuditableEntity<TKey, TUserKey>
{
    public int Version { get; set; }

    public override void SetCreatedInfo(TUserKey createdBy)
    {
        base.SetCreatedInfo(createdBy);
        Version = 0;
    }

    public override void SetUpdatedInfo(TUserKey updatedBy)
    {
        base.SetUpdatedInfo(updatedBy);
        Version++;
    }

    public void SetUpdatedInfoWithoutVersionUpdate(TUserKey updatedBy)
    {
        base.SetUpdatedInfo(updatedBy);
    }
}