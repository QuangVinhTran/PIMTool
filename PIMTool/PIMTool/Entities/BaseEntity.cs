using System.ComponentModel.DataAnnotations;

namespace PIMTool.Entities;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }

    protected BaseEntity()
    {
    }
    
}