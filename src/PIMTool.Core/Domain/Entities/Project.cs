using System.ComponentModel.DataAnnotations;

namespace PIMTool.Core.Domain.Entities
{
    public class Project : IEntity
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = null!;
    }
}