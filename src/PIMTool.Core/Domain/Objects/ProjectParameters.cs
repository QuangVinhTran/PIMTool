using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Core.Domain.Objects
{
    public class ProjectParameters
    {
        public PagingParameters? PagingParameters { get; set; }
        public string? FilterParameters { get; set; } // Name,ProjectNumber,Customer
        public string? Status { get; set; } // Filter Parameter
        public SortingParameters? SortingParameters { get; set; }
    }
}
