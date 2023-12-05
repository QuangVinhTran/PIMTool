using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Domain.Enums;

namespace PIMTool.Database
{
    public class DbInitializer
    {
        public static void Initialize(PimContext context)
        {
            if (!context.Employees.Any())
            {
                var emps = new List<Employee>()
                {
                    new Employee
                    {
                       FirstName = "Dung",
                       LastName = "Nguyen",
                       Visa = "NTD",
                       BirthDate = new DateTime(2003, 03, 09),
                       Version = new byte[0]
                    },
                    new Employee
                    {
                       FirstName = "Van",
                       LastName = "Thanh",
                       Visa = "VTV",
                       BirthDate = new DateTime(2003, 07, 14),
                       Version = new byte[0]
                    },
                    new Employee
                    {
                       FirstName = "Dat",
                       LastName = "Do",
                       Visa = "DTD",
                       BirthDate = new DateTime(2003, 10, 23),
                       Version = new byte[0]
                    },
                    new Employee
                    {
                       FirstName = "Hao",
                       LastName = "Nguyen",
                       Visa = "NVH",
                       BirthDate = new DateTime(1968, 08, 15),
                       Version = new byte[0]
                    },

                };
                context.Employees.AddRange(emps);
            }
            context.SaveChanges();
        }
    }
}
