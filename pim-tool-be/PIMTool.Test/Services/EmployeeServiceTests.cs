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

            // Act
            var entities = await _employeeService.GetEmployees();

            // Assert
            Assert.That(entities.Count(), Is.GreaterThan(0));
        }

        [Test]
        public async Task TestSearch()
        {
            // Arrange
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
            int id = 1;

            // Act
            var entity = await _employeeService.GetAsync(id);

            // Assert
            Assert.That(entity, Is.Not.Null);
        }

        [Test]
        [Ignore("This test is ignored because it will add a new employee to the database")]
        public async Task TestAddEmployee()
        {
            // Arrange
            var emp = new Employee
            {
                Visa = "ABC",
                FirstName = "Test",
                LastName = "Test",
                BirthDate = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 20))
            };

            // Act
            await _employeeService.AddAsync(emp);
            var entity = await _employeeService.GetAsync(emp.Id);
            // Assert
            Assert.That(entity, Is.Not.Null);
        }

        [Test]
        [Ignore("This test is ignored because it will update an employee in the database")]
        public async Task TestUpdateEmployee()
        {
            // Arrange
            var updateEmp = await _employeeService.GetAsync(8);
            updateEmp.FirstName = "Test FirstName Updated";
            // Act
            await _employeeService.UpdateAsync();
            var entity = await _employeeService.GetAsync(8);
            // Assert
            Assert.That(entity.FirstName, Is.EqualTo("Test FirstName Updated"));
        }

        [Test]
        [Ignore("This test is ignored because it will delete an employee from the database")]
        public async Task TestDeleteEmployee()
        {
            // Arrange
            var delEmp = await _employeeService.GetAsync(8);
            // Act
            await _employeeService.DeleteAsync(delEmp);
            var entity = await _employeeService.GetAsync(8);
            // Assert
            Assert.That(entity, Is.Null);
        }
    }
}
