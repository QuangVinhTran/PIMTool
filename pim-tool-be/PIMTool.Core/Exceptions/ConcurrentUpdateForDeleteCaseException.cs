using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Core.Exceptions
{
    public class ConcurrentUpdateForDeleteCaseException : Exception
    {
        public ConcurrentUpdateForDeleteCaseException() : base("The project has been deleted by another user!")
        {
        }
    }
}
