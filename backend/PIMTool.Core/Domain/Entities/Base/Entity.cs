namespace PIMTool.Core.Domain.Entities.Base;

public abstract class Entity<T>
{
    public T Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public bool IsTransparent()
    {
        return Equals(Id, default);
    }
}