using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Core.Domain.Objects
{
    public class PagingParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 3;
        public int? TotalPage { get; set; }
    }
}
