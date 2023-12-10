using Microsoft.Extensions.DependencyInjection;
using PIMTool.Core.Domain.Entities;
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
        public async Task Get()
        {
            // Arrange
            var project = new Project();
            await Context.AddAsync(project);
            await Context.SaveChangesAsync();

            // Act
            var result = await _projectService.GetAsync(project.Id);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}