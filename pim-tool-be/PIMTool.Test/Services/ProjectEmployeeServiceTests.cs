using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Domain.Enums;
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

            if (Context.Database.IsInMemory())
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
                Context.Employees.AddRangeAsync(emps);
                Context.SaveChangesAsync();

                var groups = new List<Group>()
                {
                    new Group
                    {
                        GroupLeaderId = emps[0].Id
                    },
                    new Group
                    {
                        GroupLeaderId = emps[1].Id
                    },
                    new Group
                    {
                        GroupLeaderId = emps[2].Id
                    },
                    new Group
                    {
                        GroupLeaderId = emps[3].Id
                    },
                };
                Context.Groups.AddRangeAsync(groups);
                Context.SaveChangesAsync();

                var projects = new List<Project>()
                {
                    new Project
                    {
                        ProjectNumber = 101,
                        Name = "Test Project 1",
                        Customer = "Test Customer 1",
                        Status = Status.INP,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 2
                    },
                    new Project
                    {
                        ProjectNumber = 102,
                        Name = "Test Project 2",
                        Customer = "Test Customer 2",
                        Status = Status.PLA,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 3
                    },
                    new Project
                    {
                        ProjectNumber = 103,
                        Name = "Test Project 3",
                        Customer = "Test Customer 3",
                        Status = Status.FIN,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 4
                    },
                    new Project
                    {
                        ProjectNumber = 104,
                        Name = "Test Project 4",
                        Customer = "Test Customer 4",
                        Status = Status.NEW,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 1
                    },
                    new Project
                    {
                        ProjectNumber = 105,
                        Name = "Test Project 5",
                        Customer = "Test Customer 5",
                        Status = Status.INP,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 2
                    },
                    new Project
                    {
                        ProjectNumber = 106,
                        Name = "Test Project 6",
                        Customer = "Test Customer 6",
                        Status = Status.PLA,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 3
                    },
                    new Project
                    {
                        ProjectNumber = 107,
                        Name = "Test Project 7",
                        Customer = "Test Customer 7",
                        Status = Status.FIN,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 4
                    },
                    new Project
                    {
                        ProjectNumber = 108,
                        Name = "Test Project 8",
                        Customer = "Test Customer 8",
                        Status = Status.NEW,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 1
                    },
                    new Project
                    {
                        ProjectNumber = 109,
                        Name = "Test Project 9",
                        Customer = "Test Customer 9",
                        Status = Status.INP,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 2
                    },
                    new Project
                    {
                        ProjectNumber = 110,
                        Name = "Test Project 10",
                        Customer = "Test Customer 10",
                        Status = Status.PLA,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 3
                    },
                    new Project
                    {
                        ProjectNumber = 111,
                        Name = "Test Project 11",
                        Customer = "Test Customer 11",
                        Status = Status.FIN,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 4
                    },
                    new Project
                    {
                        ProjectNumber = 112,
                        Name = "Test Project 12",
                        Customer = "Test Customer 12",
                        Status = Status.NEW,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 1
                    },
                    new Project
                    {
                        ProjectNumber = 113,
                        Name = "Test Project 13",
                        Customer = "Test Customer 13",
                        Status = Status.INP,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 2
                    },
                };
                Context.Projects.AddRangeAsync(projects);
                Context.SaveChangesAsync();
            }
        }

        [Test]
        public async Task TestGetProjectEmployees()
        {
            // Arrange
            var projectEmployees = new List<ProjectEmployee>()
            {
                    new ProjectEmployee
                    {
                        ProjectId = 1,
                        EmployeeId = 1
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 1,
                        EmployeeId = 2
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 2,
                        EmployeeId = 3
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 3,
                        EmployeeId = 4
                    },
            };
            await Context.ProjectEmployees.AddRangeAsync(projectEmployees);
            await Context.SaveChangesAsync();

            // Act
            var entities = await _projectEmployeeService.GetProjectEmployees();

            // Assert
            Assert.That(entities.Count(), Is.GreaterThan(0));
        }

        [Test]
        public async Task TestGetAsync()
        {
            // Arrange
            var projectEmployees = new List<ProjectEmployee>()
            {
                    new ProjectEmployee
                    {
                        ProjectId = 1,
                        EmployeeId = 1
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 1,
                        EmployeeId = 2
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 2,
                        EmployeeId = 3
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 3,
                        EmployeeId = 4
                    },
            };
            await Context.ProjectEmployees.AddRangeAsync(projectEmployees);
            await Context.SaveChangesAsync();

            int EmployeeId = 1;
            int ProjectId = 1;

            // Act
            var entity = await _projectEmployeeService.GetAsync(EmployeeId, ProjectId);

            // Assert
            Assert.That(entity, Is.Not.Null);
        }

        [Test]
        public async Task TestAddAsync()
        {
            // Arrange
            var projectEmployees = new List<ProjectEmployee>()
            {
                    new ProjectEmployee
                    {
                        ProjectId = 1,
                        EmployeeId = 1
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 1,
                        EmployeeId = 2
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 2,
                        EmployeeId = 3
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 3,
                        EmployeeId = 4
                    },
            };
            await Context.ProjectEmployees.AddRangeAsync(projectEmployees);
            await Context.SaveChangesAsync();

            var projectEmployee = new ProjectEmployee
            {
                ProjectId = 5,
                EmployeeId = 3
            };

            // Act
            await _projectEmployeeService.AddAsync(projectEmployee);
            var entity = await _projectEmployeeService.GetAsync(projectEmployee.EmployeeId, projectEmployee.ProjectId);

            // Assert
            Assert.That(entity, Is.Not.Null);
        }

        [Test]
        public async Task TestDeleteAsync()
        {
            // Arrange
            var projectEmployees = new List<ProjectEmployee>()
            {
                    new ProjectEmployee
                    {
                        ProjectId = 1,
                        EmployeeId = 1
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 1,
                        EmployeeId = 2
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 2,
                        EmployeeId = 3
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 3,
                        EmployeeId = 4
                    },
            };
            await Context.ProjectEmployees.AddRangeAsync(projectEmployees);
            await Context.SaveChangesAsync();

            // Act
            var entity = await _projectEmployeeService.GetAsync(1, 1);
            await _projectEmployeeService.DeleteAsync(entity);
            var deletedEntity = await _projectEmployeeService.GetAsync(1, 1);
            // Assert
            Assert.That(deletedEntity, Is.Null);
        }

        [Test]
        public async Task TestGetProjectMembers()
        {
            // Arrange
            var projectEmployees = new List<ProjectEmployee>()
            {
                    new ProjectEmployee
                    {
                        ProjectId = 1,
                        EmployeeId = 1
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 1,
                        EmployeeId = 2
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 1,
                        EmployeeId = 3
                    },
                    new ProjectEmployee
                    {
                        ProjectId = 3,
                        EmployeeId = 4
                    },
            };
            await Context.ProjectEmployees.AddRangeAsync(projectEmployees);
            await Context.SaveChangesAsync();
            int id = 1;

            // Act
            var entities = await _projectEmployeeService.GetProjectMembers(id);

            // Assert
            Assert.That(entities.Count(), Is.EqualTo(3));
        }
    }
}
