using PIMTool.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Core.Domain.Objects
{
    public class SortingParameters
    {
        public int? ProjectNumber { get; set; }
        public string? Name { get; set; }
        public ProjectStatus? Status { get; set; }
        public string? Customer { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
