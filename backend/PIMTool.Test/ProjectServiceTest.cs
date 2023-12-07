using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Exceptions;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Core.Models;
using PIMTool.Core.Models.Request;

namespace PIMTool.Test;

public class ProjectServiceTest : BaseTest
{
    [Test]
    public async Task GetAllProjects_Success()
    {
        var projectService = ResolveService<IProjectService>();
        var result = await projectService.GetAllProjectsAsync();
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null.And.Not.Empty);
        });
    }

    [Test]
    public async Task FindProjectsAsync_Success()
    {
        // Arrange
        var createProjectRequest = new SearchProjectsRequest()
        {
            PageIndex = 1,
            PageSize = 10,
            SearchCriteria = new SearchCriteria
            {
                ConjunctionSearchInfos = new List<SearchByInfo>
                {
                    new ()
                    {
                        FieldName = "projectNumber",
                        Value = "mock"
                    },
                    new ()
                    {
                        FieldName = "name",
                        Value = "mock"
                    },
                    new ()
                    {
                        FieldName = "customer",
                        Value = "mock"
                    },
                    new ()
                    {
                        FieldName = "Invalid field",
                        Value = "Invalid value"
                    }
                },
                DisjunctionSearchInfos = new List<SearchByInfo>
                {
                    new ()
                    {
                        FieldName = "status",
                        Value = "NEW"
                    },
                    new ()
                    {
                        FieldName = "Invalid field",
                        Value = "Invalid value"
                    }
                }
            },
            SortByInfos = new List<SortByInfo>
            {
                new ()
                {
                    FieldName = "projectNumber",
                    Ascending = true
                },
                new ()
                {
                    FieldName = "name",
                    Ascending = true
                },
                new ()
                {
                    FieldName = "status",
                    Ascending = true
                },
                new ()
                {
                    FieldName = "customer",
                    Ascending = true
                },
                new ()
                {
                    FieldName = "startDate",
                    Ascending = true
                },
                new ()
                {
                    FieldName = "invalid field name",
                    Ascending = true
                },
            },
            AdvancedFilter = new AdvancedFilter
            {
                LeaderName = "some leader name",
                MemberName = "some member name",
                StartDateRange = new DateRange()
                {
                    From = DateTime.Now,
                    To = DateTime.Now
                },
                EndDateRange = new DateRange()
                {
                    From = DateTime.Now,
                    To = DateTime.Now
                }
            }
        };
        var projectService = ResolveService<IProjectService>();
        
        // Act
        var result = await projectService.FindProjectsAsync(createProjectRequest);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.GetType().IsAssignableTo(typeof(PaginatedResult)), Is.True);
        });
    }

    [Test]
    public async Task CreateProjectAsync_Success()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new CreateProjectRequest()
        {
            ProjectNumber = 1000,
            Name = "Mock customer",
            Customer = "Mock customer",
            Status = "NEW",
            StartDate = DateTime.Now,
            GroupId = groupId,
            MemberIds = new List<Guid> {employeeId}
        };
        var dbContext = ResolveService<IAppDbContext>();
        var projectSet = dbContext.CreateSet<Project>();
        var totalBeforeCreate = await projectSet.CountAsync();
        
        // Act
        await projectService.CreateProjectAsync(request);
        var totalAfterCreate = await projectSet.CountAsync();
        
        // Assert
        Assert.That(totalAfterCreate - totalBeforeCreate, Is.EqualTo(1));
    }
    
    [Test]
    public async Task CreateProjectAsync_Success_WithExistingDeletedProject()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new CreateProjectRequest()
        {
            ProjectNumber = 100,
            Name = "Mock customer",
            Customer = "Mock customer",
            Status = "NEW",
            StartDate = DateTime.Now,
            GroupId = groupId
        };
        var dbContext = ResolveService<IAppDbContext>();
        var projectSet = dbContext.CreateSet<Project>();
        var totalBeforeCreate = await projectSet.CountAsync();
        
        // Act
        await projectService.CreateProjectAsync(request);
        var totalAfterCreate = await projectSet.CountAsync();
        
        // Assert
        Assert.That(totalAfterCreate - totalBeforeCreate, Is.EqualTo(0));
    }
    
    [Test]
    public void CreateProjectAsync_Failure_ProjectNumberAlreadyExists()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new CreateProjectRequest()
        {
            ProjectNumber = 1,
            Name = "Mock customer",
            Customer = "Mock customer",
            Status = "NEW",
            StartDate = DateTime.Now,
            GroupId = groupId
        };
        
        // Act
        // Assert
        Assert.ThrowsAsync<ProjectNumberAlreadyExistsException>(async () => await projectService.CreateProjectAsync(request));
    }
    
    [Test]
    public void CreateProjectAsync_Failure_GroupDoesNotExist()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new CreateProjectRequest()
        {
            ProjectNumber = 1000,
            Name = "Mock customer",
            Customer = "Mock customer",
            Status = "NEW",
            StartDate = DateTime.Now,
            GroupId = Guid.NewGuid()
        };
        
        // Act
        // Assert
        Assert.ThrowsAsync<GroupDoesNotExistException>(async () => await projectService.CreateProjectAsync(request));
    }
    
    [Test]
    public void CreateProjectAsync_Failure_MemberDoesNotExist()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new CreateProjectRequest()
        {
            ProjectNumber = 1000,
            Name = "Mock customer",
            Customer = "Mock customer",
            Status = "NEW",
            StartDate = DateTime.Now,
            GroupId = groupId,
            MemberIds = new List<Guid> { Guid.NewGuid() }
        };
        
        // Act
        // Assert
        Assert.ThrowsAsync<EmployeeDoesNotExistException>(async () => await projectService.CreateProjectAsync(request));
    }
    
    [Test]
    public async Task CheckIfProjectNumberExistsAsync_Success()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        
        // Act
        var result = await projectService.CheckIfProjectNumberExistsAsync(0);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.True);
        });
    }

    [Test]
    public async Task CheckIfProjectNumberExistsAsync_Failure()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        
        // Act
        var result = await projectService.CheckIfProjectNumberExistsAsync(1);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.False);
        });
    }

    [Test]
    public async Task UpdateProjectsAsync_Success()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new UpdateProjectRequest
        {
            Name = "Updated project name",
            Customer = "Updated customer",
            Status = "FIN",
            StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
            EndDate = DateTime.Now,
            GroupId = groupId,
            MemberIds = new List<Guid> { employeeId },
            Version = 0
        };

        // Act
        var result = await projectService.UpdateProjectAsync(request, projectId, userId.ToString());

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void UpdateProjectAsync_Failure_InvalidUpdaterId()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new UpdateProjectRequest
        {
            Name = "Updated project name",
            Customer = "Updated customer",
            Status = "FIN",
            StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
            EndDate = DateTime.Now,
            GroupId = groupId,
            MemberIds = new List<Guid> { employeeId },
            Version = 0
        };

        // Act
        // Assert
        Assert.ThrowsAsync<InvalidGuidIdException>(async () =>
            await projectService.UpdateProjectAsync(request, projectId, "Invalid guid"));
    }

    [Test]
    public void UpdateProjectAsync_Failure_UpdaterDoesNotExist()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new UpdateProjectRequest
        {
            Name = "Updated project name",
            Customer = "Updated customer",
            Status = "FIN",
            StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
            EndDate = DateTime.Now,
            GroupId = groupId,
            MemberIds = new List<Guid> { employeeId },
            Version = 0
        };

        // Act
        // Assert
        Assert.ThrowsAsync<UserDoesNotExistException>(async () =>
            await projectService.UpdateProjectAsync(request, projectId, Guid.NewGuid().ToString()));
    }

    [Test]
    public void UpdateProjectAsync_Failure_ProjectIdDoesNotExist()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new UpdateProjectRequest
        {
            Name = "Updated project name",
            Customer = "Updated customer",
            Status = "FIN",
            StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
            EndDate = DateTime.Now,
            GroupId = groupId,
            MemberIds = new List<Guid> { employeeId },
            Version = 0
        };

        // Act
        // Assert
        Assert.ThrowsAsync<ProjectDoesNotExistException>(async () =>
            await projectService.UpdateProjectAsync(request, Guid.NewGuid(), userId.ToString()));
    }

    [Test]
    public void UpdateProjectAsync_Failure_MemberDoesNotExist()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new UpdateProjectRequest
        {
            Name = "Updated project name",
            Customer = "Updated customer",
            Status = "FIN",
            StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
            EndDate = DateTime.Now,
            GroupId = groupId,
            MemberIds = new List<Guid> { employeeId, Guid.NewGuid() },
            Version = 0
        };

        // Act
        // Assert
        Assert.ThrowsAsync<EmployeeDoesNotExistException>(async () =>
            await projectService.UpdateProjectAsync(request, projectId, userId.ToString()));
    }

    [Test]
    public void UpdateProjectAsync_Failure_VersionMismatched()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new UpdateProjectRequest
        {
            Name = "Updated project name",
            Customer = "Updated customer",
            Status = "FIN",
            StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
            EndDate = DateTime.Now,
            GroupId = groupId,
            MemberIds = new List<Guid> { employeeId },
            Version = 1
        };

        // Act
        // Assert
        Assert.ThrowsAsync<VersionMismatchedException>(async () =>
            await projectService.UpdateProjectAsync(request, projectId, userId.ToString()));
    }

    [Test]
    public async Task DeleteProjectAsync_Success()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        
        // Act
        var result = await projectService.DeleteProjectAsync(projectId);
        
        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void DeleteProjectAsync_Failure_ProjectStatusIsNotNew()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        
        // Act
        // Assert
        Assert.ThrowsAsync<IndelibleProjectException>(async () =>
            await projectService.DeleteProjectAsync(plannedProjectId));
    }
    
    [Test]
    public void DeleteProjectAsync_Failure_ProjectIdDoesNotExist()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        
        // Act
        // Assert
        Assert.ThrowsAsync<ProjectDoesNotExistException>(async () =>
            await projectService.DeleteProjectAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task FindProjectByProjectNumberAsync_Success()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        
        // Act
        var result = await projectService.FindProjectByProjectNumberAsync(1);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        });
    }
    
    [Test]
    public void FindProjectByProjectNumberAsync_Failure_ProjectNumberDoesNotExist()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        
        // Act
        // Assert
        Assert.ThrowsAsync<ProjectDoesNotExistException>(async () =>
            await projectService.FindProjectByProjectNumberAsync(9999));
    }

    [Test]
    public async Task DeleteMultipleProjectsAsync_Success()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new DeleteMultipleProjectsRequest()
        {
            ProjectIds = new List<Guid> { projectId }
        };
        
        // Act
        var result = await projectService.DeleteMultipleProjectsAsync(request);
        
        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
    
    [Test]
    public void DeleteMultipleProjectsAsync_Failure_ProjectIdDoesNotExist()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new DeleteMultipleProjectsRequest()
        {
            ProjectIds = new List<Guid> { projectId, Guid.NewGuid() }
        };
        
        // Act
        // Assert
        Assert.ThrowsAsync<ProjectDoesNotExistException>(async () =>
            await projectService.DeleteMultipleProjectsAsync(request));
    }
    
    [Test]
    public void DeleteMultipleProjectsAsync_Failure_ProjectStatusIsNotNew()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new DeleteMultipleProjectsRequest()
        {
            ProjectIds = new List<Guid> { projectId, plannedProjectId }
        };
        
        // Act
        // Assert
        Assert.ThrowsAsync<IndelibleProjectException>(async () =>
            await projectService.DeleteMultipleProjectsAsync(request));
    }
    
    [Test]
    public async Task ImportProjectsFromFileNpoiAsync_Success()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        const string fileName = "Projects.xlsx";
        var fileMock = new Mock<IFormFile>();
        var fs = File.OpenRead(fileName);
        fs.Position = 0;
        fileMock.Setup(f => f.OpenReadStream()).Returns(fs);
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(fs.Length);
        
        // Act
        var result = await projectService.ImportProjectsFromFileNpoiAsync(fileMock.Object);

        // Assert
        Assert.That(result.FileStream.Length, Is.Zero);
    }
    
    [Test]
    public void ImportProjectsFromFileNpoiAsync_Failure_UnsupportedFileExtension()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        const string fileName = "Projects.xlsx";
        var fileMock = new Mock<IFormFile>();
        var fs = File.OpenRead(fileName);
        fs.Position = 0;
        fileMock.Setup(f => f.OpenReadStream()).Returns(fs);
        fileMock.Setup(f => f.FileName).Returns("Projects.txt");
        fileMock.Setup(f => f.Length).Returns(fs.Length);
        
        // Act
        // Assert
        Assert.ThrowsAsync<UnsupportedFileExtensionException>(async () =>
            await projectService.ImportProjectsFromFileNpoiAsync(fileMock.Object));
    }

    [Test]
    public async Task ImportProjectsFromFileNpoiAsync_Failure_InvalidData()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        const string fileName = "Invalid_Projects.xlsx";
        var fileMock = new Mock<IFormFile>();
        await using var fs = File.OpenRead(fileName);
        fs.Position = 0;
        fileMock.Setup(f => f.OpenReadStream()).Returns(fs);
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(fs.Length);
        
        // Act
        var result = await projectService.ImportProjectsFromFileNpoiAsync(fileMock.Object);
        
        // Assert
        Assert.That(result.FileStream.Length, Is.GreaterThan(1000));
    }

    [Test]
    public async Task ExportProjectsToFileAsync_Success()
    {
        // Arrange
        var projectService = ResolveService<IProjectService>();
        var request = new ExportProjectsToFileRequest()
        {
            ProjectName = "Project",
            Customer = "Customer",
            Status = "NEW",
            LeaderName = "Employee",
            OrderBy = "projectNumber",
            NumberOfRows = 10,
            StartDateFrom = DateTime.Now.Subtract(TimeSpan.FromDays(8)),
            StartDateTo = DateTime.Now,
            EndDateFrom = DateTime.Now,
            EndDateTo = DateTime.Now.Add(TimeSpan.FromDays(8))
        };
        
        // Act
        var result = await projectService.ExportProjectsToFileAsync(request);

        // Assert
        Assert.That(result.FileStream.Length, Is.GreaterThan(1000));
    }
}