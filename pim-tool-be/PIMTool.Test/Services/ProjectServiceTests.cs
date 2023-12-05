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
        }

        [Test]
        public async Task TestGetProjects()
        {
            // Arrange

            // Act
            var entities = await _projectService.GetProjects();

            // Assert
            Assert.That(entities.Count(), Is.GreaterThan(0));
        }

        [Test]
        public async Task TestGetProjectsPagination()
        {
            // Arrange
            int limit = 10;
            int skip = 0;
            // Act
            var entities = await _projectService.GetProjectsPagination(skip, limit);

            // Assert
            Assert.That(entities.Count(), Is.EqualTo(10));
        }

        [Test]
        public async Task TestSearchWithPagination()
        {
            // Arrange
            int limit = 10;
            int skip = 0;
            string searchVal = "Code";
            int status = 0;
            // Act
            var entities = await _projectService.SearchWithPagination(searchVal, status, skip, limit);

            // Assert
            Assert.That(entities.Count(), Is.AtLeast(1));
        }

        [Test]
        public async Task TestSearch()
        {
            // Arrange
            string searchVal = "Code";
            int status = 0;
            // Act
            var entities = await _projectService.Search(searchVal, status);

            // Assert
            Assert.That(entities.Count(), Is.AtLeast(1));
        }

        [Test]
        public async Task TestGetByProjectNumber()
        {
            // Arrange
            int projectNumber = 100;
            // Act
            var entity = await _projectService.GetByProjectNumber(projectNumber);

            // Assert
            Assert.That(entity, Is.Not.Null);
        }

        [Test]
        public async Task TestGetAsync()
        {
            // Arrange
            int id = 35;
            // Act
            var entity = await _projectService.GetAsync(id);

            // Assert
            Assert.That(entity, Is.Not.Null);
        }

        [Test]
        public async Task TestAddProjectWithExistedNumber()
        {
            // Arrange
            var newProject = new Project
            {
                ProjectNumber = 100,
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
        [Ignore("Ignore a test")]
        public async Task TestAddProjectSuccessfully()
        {
            // Arrange
            var newProject = new Project
            {
                ProjectNumber = 2002,
                Name = "Test Project",
                Customer = "Test Customer",
                Status = 0,
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
        [Ignore("Ignore a test")]
        public async Task TestUpdateProjectSuccessfully()
        {
            // Arrange
            var updateProject = await _projectService.GetAsync(35);
            updateProject.Name = "Test Project Updated";
            // Act
            await _projectService.UpdateAsync();
            var entity = await _projectService.GetAsync(35);
            // Assert
            Assert.That(entity.Name, Is.EqualTo("Test Project Updated"));
        }

        [Test]
        [Ignore("Ignore a test")]
        public async Task TestDeleteProjectSuccessfully()
        {
            // Arrange
            var deleteProject = await _projectService.GetByProjectNumber(2000);
            // Act
            await _projectService.DeleteAsync(deleteProject);
            var entity = await _projectService.GetByProjectNumber(2000);
            // Assert
            Assert.That(entity, Is.Null);
        }
    }
}