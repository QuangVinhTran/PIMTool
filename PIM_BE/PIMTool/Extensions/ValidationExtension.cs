using PIMTool.Core.Domain.Entities;

namespace PIMTool.Extensions;

public static class ValidationExtension
{
    public static bool UniqueProjectNumber(this Project project, IEnumerable<Project> listProject)
    {
        bool check = true;
        foreach(var p in listProject)
        {
            if (p.ProjectNumber == project.ProjectNumber)
            {
                check = false;
                break;
            } 
        }
        return check;
    }
    public static bool NoLeaderGroup(this Employee employee, IEnumerable<Group> listGroup)
    {
        bool check = true;
        foreach(var group in listGroup)
        {
            if(employee.Id == group.GroupLeaderId)
            {
                check = false;
                break;
            }
        }
        return check;
    }
}
