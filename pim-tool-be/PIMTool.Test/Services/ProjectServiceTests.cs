using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Domain.Enums;
using PIMTool.Core.Exceptions;
using PIMTool.Core.Interfaces.Services;

namespace PIMTool.Test.Services
{
    public class ProjectServiceTests : BaseTest
    {
        private IProjectService _projectService = null!;

        [SetUp]
        public void SetUp()
        {
            _projectService = ServiceProvider.GetRequiredService<IProjectService>();
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
            }
        }

        [Test]
        public async Task Get()
        {
            // Arrange
            var newProject = new Project
            {
                ProjectNumber = 100,
                Name = "Test Project",
                Customer = "Test Customer",
                Status = Status.FIN,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                GroupId = 2
            };
            await Context.Projects.AddAsync(newProject);
            await Context.SaveChangesAsync();

            // Act
            var result = await _projectService.GetAsync(newProject.Id);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task TestGetProjects()
        {
            // Arrange
            var projects = new List<Project>()
                {
                    new Project
                    {
                        ProjectNumber = 100,
                        Name = "Test Project 1",
                        Customer = "Test Customer 1",
                        Status = Status.NEW,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 1
                    },
                    new Project
                    {
                        ProjectNumber = 101,
                        Name = "Test Project 2",
                        Customer = "Test Customer 2",
                        Status = Status.INP,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 2
                    },
                    new Project
                    {
                        ProjectNumber = 102,
                        Name = "Test Project 3",
                        Customer = "Test Customer 3",
                        Status = Status.PLA,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 3
                    }
                };
            await Context.Projects.AddRangeAsync(projects);
            await Context.SaveChangesAsync();

            // Act
            var entities = await _projectService.GetProjects();

            // Assert
            Assert.That(entities.Count(), Is.GreaterThan(0));
        }

        [Test]
        public async Task TestGetProjectsPagination()
        {
            // Arrange
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
            await Context.Projects.AddRangeAsync(projects);
            await Context.SaveChangesAsync();

            int limit = 10;
            int skip = 10;
            // Act
            var entities = await _projectService.GetProjectsPagination(skip, limit);

            // Assert
            Assert.That(entities.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task TestSearchWithPagination()
        {
            // Arrange
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
                        Name = "aaaa",
                        Customer = "aaaa",
                        Status = Status.NEW,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 1
                    },
                    new Project
                    {
                        ProjectNumber = 105,
                        Name = "bbbb",
                        Customer = "bbbb",
                        Status = Status.INP,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 2
                    }
                };
            await Context.Projects.AddRangeAsync(projects);
            await Context.SaveChangesAsync();
            int limit = 10;
            int skip = 0;
            string searchVal = "Test";
            int? status = null;
            // Act
            var entities = await _projectService.SearchWithPagination(searchVal, status, skip, limit);

            // Assert
            Assert.That(entities.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task TestSearch()
        {
            // Arrange
            var projects = new List<Project>()
                {
                    new Project
                    {
                        ProjectNumber = 101,
                        Name = "Test Project 1",
                        Customer = "Test Customer 1",
                        Status = Status.NEW,
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
                        Status = Status.NEW,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 4
                    },
                    new Project
                    {
                        ProjectNumber = 104,
                        Name = "aaaa",
                        Customer = "aaaa",
                        Status = Status.NEW,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 1
                    },
                    new Project
                    {
                        ProjectNumber = 105,
                        Name = "bbbb",
                        Customer = "bbbb",
                        Status = Status.INP,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        GroupId = 2
                    }
                };
            await Context.Projects.AddRangeAsync(projects);
            await Context.SaveChangesAsync();
            string searchVal = "Test";
            int status = 0;
            // Act
            var entities = await _projectService.Search(searchVal, status);

            // Assert
            Assert.That(entities.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task TestGetByProjectNumber()
        {
            // Arrange
            var projects = new List<Project>()
                {
                    new Project
                    {
                        ProjectNumber = 101,
                        Name = "Test Project 1",
                        Customer = "Test Customer 1",
                        Status = Status.NEW,
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
                    }
                };
            await Context.Projects.AddRangeAsync(projects);
            await Context.SaveChangesAsync();
            int projectNumber = 101;
            // Act
            var entity = await _projectService.GetByProjectNumber(projectNumber);

            // Assert
            Assert.That(entity, Is.Not.Null);
        }

        [Test]
        public async Task TestAddProjectWithExistedNumber()
        {
            // Arrange
            var projects = new List<Project>()
                {
                    new Project
                    {
                        ProjectNumber = 101,
                        Name = "Test Project 1",
                        Customer = "Test Customer 1",
                        Status = Status.NEW,
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
                    }
                };
            await Context.Projects.AddRangeAsync(projects);
            await Context.SaveChangesAsync();

            var newProject = new Project
            {
                ProjectNumber = 101,
                Name = "Test Project",
                Customer = "Test Customer",
                Status = 0,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                GroupId = 1
            };
            // Act

            // Assert
            Assert.ThrowsAsync<ProjectNumberAlreadyExistsException>(async () => await _projectService.AddAsync(newProject));
        }

        [Test]
        public async Task TestAddProjectSuccessfully()
        {
            // Arrange
            var newProject = new Project
            {
                ProjectNumber = 100,
                Name = "Test Project",
                Customer = "Test Customer",
                Status = Status.NEW,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                GroupId = 1
            };
            // Act
            var entity = await _projectService.AddAsync(newProject);
            // Assert
            Assert.That(entity, Is.Not.Null);
        }

        [Test]
        public async Task TestUpdateProjectSuccessfully()
        {
            // Arrange
            var projects = new List<Project>()
                {
                    new Project
                    {
                        ProjectNumber = 101,
                        Name = "Test Project 1",
                        Customer = "Test Customer 1",
                        Status = Status.NEW,
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
                    }
                };
            await Context.Projects.AddRangeAsync(projects);
            await Context.SaveChangesAsync();

            var updateProject = await _projectService.GetAsync(projects[0].Id);
            updateProject.Name = "Test Project Updated";
            // Act
            await _projectService.UpdateAsync();
            var entity = await _projectService.GetAsync(projects[0].Id);
            // Assert
            Assert.That(entity.Name, Is.EqualTo("Test Project Updated"));
        }

        [Test]
        public async Task TestDeleteProjectSuccessfully()
        {
            // Arrange
            var projects = new List<Project>()
                {
                    new Project
                    {
                        ProjectNumber = 101,
                        Name = "Test Project 1",
                        Customer = "Test Customer 1",
                        Status = Status.NEW,
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
                    }
                };
            await Context.Projects.AddRangeAsync(projects);
            await Context.SaveChangesAsync();

            var deleteProject = await _projectService.GetByProjectNumber(102);
            // Act
            await _projectService.DeleteAsync(deleteProject);
            var entity = await _projectService.GetProjects();
            // Assert
            Assert.That(entity.Count(), Is.EqualTo(1));
        }
    }
}