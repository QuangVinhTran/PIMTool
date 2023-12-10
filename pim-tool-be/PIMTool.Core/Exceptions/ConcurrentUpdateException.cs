using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Core.Exceptions
{
    public class ConcurrentUpdateException : Exception
    {
        public ConcurrentUpdateException() : base("The project has been updated by another user. Please refresh the page and try again")
        {
        }
    }
}
