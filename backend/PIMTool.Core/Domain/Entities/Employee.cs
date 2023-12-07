using System;
using System.Collections.Generic;
using PIMTool.Core.Domain.Entities.Base;

namespace PIMTool.Core.Domain.Entities;

public class Employee : VersionableEntity<Guid, Guid>
{
    public string Visa { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime BirthDate { get; set; }
    
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}