using Microsoft.Extensions.DependencyInjection;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Domain.Objects;
using PIMTool.Core.Interfaces.Services;

namespace PIMTool.Test.Services
{
    public class ProjectServiceTests : BaseTest
    {
        private IProjectService _projectService = null!;
        private IEmployeeService _employeeService = null!;

        [SetUp]
        public void SetUp()
        {
            _projectService = ServiceProvider.GetRequiredService<IProjectService>();
            _employeeService = ServiceProvider.GetRequiredService<IEmployeeService>();
        }

        [Test]
        public async Task AddAsync()
        {
            // Arrange
            var employee = new Employee()
            {
                Visa = "LTQ",
                FirstName = "Quyen",
                LastName = "Le",
                BirthDate = new DateTime(2022, 12, 6)
            };
            await _employeeService.AddAsync(employee);

            var project = new Project()
            {
                GroupId = 1,
                ProjectNumber = 1,
                Name = "Test Project",
                Customer = "ELCA",
                Status = ProjectStatus.NEW,
                StartDate = DateTime.Now.Date
            };

            List<string> visa = new List<string>();
            visa.Add("LTQ");

            // Act
            await _projectService.AddAsync(project, visa);

            
            var result = await _projectService.GetAsync(project.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.GroupId, Is.EqualTo(1));
            Assert.That(result.ProjectNumber, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Test Project"));
            Assert.That(result.Customer, Is.EqualTo("ELCA"));
            Assert.That(result.Status, Is.EqualTo(Enum.Parse(typeof(ProjectStatus), "NEW")));
            Assert.That(result.StartDate, Is.EqualTo(DateTime.Now.Date));

        }

        [Test]
        public async Task Get()
        {
            // Arrange
            var project = new Project(){
                GroupId = 1,
                ProjectNumber = 1,
                Name = "Test Project",
                Customer = "ELCA",
                Status = ProjectStatus.NEW,
                StartDate = DateTime.Now.Date
            };
            await Context.AddAsync(project);
            await Context.SaveChangesAsync();

            // Act
            var result = await _projectService.GetAsync(project.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.GroupId, Is.EqualTo(1));
            Assert.That(result.ProjectNumber, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Test Project"));
            Assert.That(result.Customer, Is.EqualTo("ELCA"));
            Assert.That(result.Status, Is.EqualTo(Enum.Parse(typeof(ProjectStatus), "NEW")));
            Assert.That(result.StartDate, Is.EqualTo(DateTime.Now.Date));

        }

        [Test]
        public async Task GetWithPaginationAndFilter()
        {
            // Arrange
            Project[] projects = new Project[]
            {
                new Project(){ GroupId = 1, ProjectNumber = 3316, Name = "GKB", Customer = "FPT", Status = ProjectStatus.NEW, StartDate = DateTime.Now.Date },
                new Project(){ GroupId = 2, ProjectNumber = 4464, Name = "MGB", Customer = "ELCA", Status = ProjectStatus.NEW, StartDate = DateTime.Now.Date },
                new Project(){ GroupId = 3, ProjectNumber = 2134, Name = "Fact", Customer = "ELCA", Status = ProjectStatus.NEW, StartDate = DateTime.Now.Date },
            };
            await Context.AddRangeAsync(projects);
            await Context.SaveChangesAsync();

            ProjectParameters projectParameters = new()
            {
                PagingParameters = new PagingParameters() { PageNumber = 1 , PageSize = 2},
                FilterParameters = "ELCA",
                Status = "NEW"
            };

            // Act
            var result = _projectService.Get(projectParameters) .ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result[0].ProjectNumber, Is.EqualTo(4464));
            Assert.That(result[0].Name, Is.EqualTo("MGB"));
            Assert.That(result[0].Customer, Is.EqualTo("ELCA"));
            Assert.That(result[0].Status, Is.EqualTo(Enum.Parse(typeof(ProjectStatus), "NEW")));
            Assert.That(result[0].StartDate, Is.EqualTo(DateTime.Now.Date));

        }

        [Test]
        public async Task Delete()
        {
            // Arrange
            var project = new Project()
            {
                GroupId = 1,
                ProjectNumber = 1,
                Name = "Test Project",
                Customer = "ELCA",
                Status = ProjectStatus.NEW,
                StartDate = DateTime.Now.Date
            };
            await Context.AddAsync(project);
            await Context.SaveChangesAsync();

            // Act
            var deletedProject = await _projectService.GetAsync(project.Id);
            await _projectService.Delete(deletedProject);
            var result = await Context.Projects.FindAsync(project.Id);

            // Assert
            Assert.IsNull(result);

        }

        [Test]
        public async Task DeleteRange()
        {
            // Arrange
            Project[] projects = new Project[]
            {
                new Project(){ GroupId = 1, ProjectNumber = 3316, Name = "GKB", Customer = "FPT", Status = ProjectStatus.FIN, StartDate = DateTime.Now.Date },
                new Project(){ GroupId = 2, ProjectNumber = 4464, Name = "MGB", Customer = "ELCA", Status = ProjectStatus.FIN, StartDate = DateTime.Now.Date },
                new Project(){ GroupId = 3, ProjectNumber = 2134, Name = "Fact", Customer = "ELCA", Status = ProjectStatus.FIN, StartDate = DateTime.Now.Date },
            };
            await Context.AddRangeAsync(projects);
            await Context.SaveChangesAsync();

            ProjectParameters projectParameters = new()
            {
                PagingParameters = new PagingParameters() { PageNumber = 1, PageSize = 3 },
                Status = "FIN"
            };

            // Act
            var deletingProjects = _projectService.Get(projectParameters).ToArray();
            await _projectService.DeleteRange(deletingProjects);
            var result = _projectService.Get(projectParameters).ToArray();

            // Assert
            Assert.IsEmpty(result);


        }

        [Test]
        public async Task Update()
        {
            // Arrange
            var employee = new Employee()
            {
                Visa = "LTQ",
                FirstName = "Quyen",
                LastName = "Le",
                BirthDate = new DateTime(2022, 12, 6)
            };
            await _employeeService.AddAsync(employee);

            var project = new Project()
            {
                GroupId = 1,
                ProjectNumber = 1,
                Name = "Test Project",
                Customer = "ELCA",
                Status = ProjectStatus.NEW,
                StartDate = DateTime.Now.Date
            };

            List<string> visa = new List<string>();
            visa.Add("LTQ");

            // Act
            await _projectService.AddAsync(project, visa);


            var updatingProject = await _projectService.GetAsync(project.Id);
            updatingProject.Name = "Update Test Project";
            updatingProject.Status = ProjectStatus.INP;
            await _projectService.Update(updatingProject, visa);
            var result = await _projectService.GetAsync(project.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.GroupId, Is.EqualTo(1));
            Assert.That(result.ProjectNumber, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Update Test Project"));
            Assert.That(result.Customer, Is.EqualTo("ELCA"));
            Assert.That(result.Status, Is.EqualTo(Enum.Parse(typeof(ProjectStatus), "INP")));
            Assert.That(result.StartDate, Is.EqualTo(DateTime.Now.Date));

        }

        [Test]
        public async Task SearchProjectNumber()
        {
            // Arrange
            var project = new Project()
            {
                GroupId = 1,
                ProjectNumber = 1,
                Name = "Test Project",
                Customer = "ELCA",
                Status = ProjectStatus.NEW,
                StartDate = DateTime.Now.Date
            };
            await Context.AddAsync(project);
            await Context.SaveChangesAsync();

            // Act
            var result = _projectService.SearchProjectByNumber(project.ProjectNumber).FirstOrDefault();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.GroupId, Is.EqualTo(1));
            Assert.That(result.ProjectNumber, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Test Project"));
            Assert.That(result.Customer, Is.EqualTo("ELCA"));
            Assert.That(result.Status, Is.EqualTo(Enum.Parse(typeof(ProjectStatus), "NEW")));
            Assert.That(result.StartDate, Is.EqualTo(DateTime.Now.Date));

        }
    }
}