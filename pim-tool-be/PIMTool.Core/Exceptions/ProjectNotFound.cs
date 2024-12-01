using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Core.Exceptions
{
    public class ProjectNotFound : Exception
    {
        public ProjectNotFound() : base("The project was not found. Please select a different project")
        {
        }
    }
}
