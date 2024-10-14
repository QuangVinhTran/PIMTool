namespace PIMTool.Core.Domain.Entities;

public interface IEntity
{
    public int Id { get; set; }
    public byte[] Version { get; set; }
}