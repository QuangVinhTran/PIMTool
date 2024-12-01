using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Core.Exceptions
{
    public class ProjectNumberAlreadyExistsException : Exception
    {
        public ProjectNumberAlreadyExistsException() : base("The project number already existed. Please select a different project number")
        {
        }
    }
}
