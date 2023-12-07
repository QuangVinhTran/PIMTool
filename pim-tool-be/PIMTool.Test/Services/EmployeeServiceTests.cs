using Microsoft.Extensions.DependencyInjection;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Test.Services
{
    public class EmployeeServiceTests : BaseTest
    {
        private IEmployeeService _employeeService = null!;

        [SetUp]
        public void SetUp()
        {
            _employeeService = ServiceProvider.GetRequiredService<IEmployeeService>();
        }

        [Test]
        public async Task TestGetEmployees()
        {
            // Arrange
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
            await Context.Employees.AddRangeAsync(emps);
            await Context.SaveChangesAsync();

            // Act
            var entities = await _employeeService.GetEmployees();

            // Assert
            Assert.That(entities.Count(), Is.GreaterThan(0));
        }

        [Test]
        public async Task TestSearch()
        {
            // Arrange
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
            await Context.Employees.AddRangeAsync(emps);
            await Context.SaveChangesAsync();
            string searchVal = "u";

            // Act
            var entities = await _employeeService.Search(searchVal);

            // Assert
            Assert.That(entities.Count(), Is.AtLeast(1));
        }

        [Test]
        public async Task TestGetAsync()
        {
            // Arrange
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
            await Context.Employees.AddRangeAsync(emps);
            await Context.SaveChangesAsync();
            int id = emps[0].Id;

            // Act
            var entity = await _employeeService.GetAsync(id);

            // Assert
            Assert.That(entity, Is.Not.Null);
        }

        [Test]
        public async Task TestAddEmployee()
        {
            // Arrange
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
            await Context.Employees.AddRangeAsync(emps);
            await Context.SaveChangesAsync();

            var emp = new Employee
            {
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                Visa = "ABC",
                BirthDate = new DateTime(2003, 03, 09),
                Version = new byte[0]
            };
            

            // Act
            await _employeeService.AddAsync(emp);
            await Context.SaveChangesAsync();
            var entities = await _employeeService.GetEmployees();
            // Assert
            Assert.That(entities.Count(), Is.EqualTo(5));
        }

        [Test]
        public async Task TestUpdateEmployee()
        {
            // Arrange
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
            await Context.Employees.AddRangeAsync(emps);
            await Context.SaveChangesAsync();

            var updateEmp = await _employeeService.GetAsync(emps[0].Id);
            updateEmp.FirstName = "Test FirstName Updated";

            // Act
            await _employeeService.UpdateAsync();
            var entity = await _employeeService.GetAsync(emps[0].Id);

            // Assert
            Assert.That(entity.FirstName, Is.EqualTo("Test FirstName Updated"));
        }

        [Test]
        public async Task TestDeleteEmployee()
        {
            // Arrange
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
            await Context.Employees.AddRangeAsync(emps);
            await Context.SaveChangesAsync();
            var delEmp = await _employeeService.GetAsync(emps[0].Id);
            // Act
            await _employeeService.DeleteAsync(delEmp);
            var entities = await _employeeService.GetEmployees();
            // Assert
            Assert.That(entities.Count(), Is.EqualTo(3));
        }
    }
}
