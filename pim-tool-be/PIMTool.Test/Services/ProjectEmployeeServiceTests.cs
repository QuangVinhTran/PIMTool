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
    public class ProjectEmployeeServiceTests : BaseTest
    {
        private IProjectEmployeeService _projectEmployeeService = null!;

        [SetUp]
        public void SetUp()
        {
            _projectEmployeeService = ServiceProvider.GetRequiredService<IProjectEmployeeService>();
        }

        [Test]
        public async Task TestGetProjectEmployees()
        {
            // Arrange

            // Act
            var entities = await _projectEmployeeService.GetProjectEmployees();

            // Assert
            Assert.That(entities.Count(), Is.GreaterThan(0));
        }

        [Test]
        public async Task TestGetAsync()
        {
            // Arrange
            int EmployeeId = 1;
            int ProjectId = 36;

            // Act
            var entity = await _projectEmployeeService.GetAsync(EmployeeId, ProjectId);

            // Assert
            Assert.That(entity, Is.Not.Null);
        }

        [Test]
        [Ignore("This test is ignored because it is not implemented yet.")]
        public async Task TestAddAsync()
        {
            // Arrange
            var projectEmployee = new ProjectEmployee
            {
                ProjectId = 88,
                EmployeeId = 1
            };

            // Act
            await _projectEmployeeService.AddAsync(projectEmployee);

            // Assert
            Assert.That(projectEmployee.ProjectId, Is.GreaterThan(0));
        }

        [Test]
        [Ignore("This test is ignored because it is not implemented yet.")]
        public async Task TestDeleteAsync()
        {
            // Arrange

            // Act
            var entity = await _projectEmployeeService.GetAsync(1,88);
            await _projectEmployeeService.DeleteAsync(entity);

            // Assert
            Assert.That(entity, Is.Not.Null);
        }

        [Test]
        public async Task TestGetProjectMembers()
        {
            // Arrange
            int id = 36;

            // Act
            var entities = await _projectEmployeeService.GetProjectMembers(id);

            // Assert
            Assert.That(entities.Count(), Is.GreaterThan(0));
        }
    }
}
