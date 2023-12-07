using PIMTool.Core.Exceptions;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Core.Models;
using PIMTool.Core.Models.Request;

namespace PIMTool.Test;

public class GroupServiceTest : BaseTest
{
    [Test]
    public async Task GetAllGroupsAsync_Success()
    {
        // Arrange
        var groupService = ResolveService<IGroupService>();
        
        // Act
        var result = await groupService.GetAllGroupsAsync();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null.And.Not.Empty);
        });
    }

    [Test]
    public async Task CreateGroupAsync_Success()
    {
        // Arrange 
        var groupService = ResolveService<IGroupService>();
        var request = new CreateGroupRequest()
        {
            Name = "Mock group",
            LeaderId = employeeId
        };

        // Act
        var result = await groupService.CreateGroupAsync(request);
        
        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task DeleteGroupAsync_Success()
    {
        // Arrange
        var groupService = ResolveService<IGroupService>();
        
        // Act
        var result = await groupService.DeleteGroupAsync(groupId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
    
    [Test]
    public void DeleteGroupAsync_Failure_GroupIdDoesNotExist()
    {
        // Arrange
        var groupService = ResolveService<IGroupService>();
        
        // Act
        // Assert
        Assert.ThrowsAsync<GroupDoesNotExistException>(async () => await groupService.DeleteGroupAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task FindGroupAsync_Success()
    {
        // Arrange
        var groupService = ResolveService<IGroupService>();

        // Act
        var result = await groupService.FindGroupAsync(groupId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        });
    }
    
    [Test]
    public void FindGroupAsync_Failure_GroupIdDoesNotExist()
    {
        // Arrange
        var groupService = ResolveService<IGroupService>();

        // Act
        // Assert
        Assert.ThrowsAsync<GroupDoesNotExistException>(async () => await groupService.FindGroupAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task FindGroupsAsync_Success()
    {
        // Arrange
        var groupService = ResolveService<IGroupService>();
        var request = new SearchGroupsRequest()
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
                    }
                },
                DisjunctionSearchInfos = new List<SearchByInfo>
                {
                    new ()
                    {
                        FieldName = "status",
                        Value = "NEW"
                    }
                }
            },
            SortByInfos = new List<SortByInfo>
            {
                new ()
                {
                    FieldName = "projectNumber",
                    Ascending = true
                }
            }
        };
        
        // Act
        var result = await groupService.FindGroupsAsync(request);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null.And.Not.Empty);
        });
    }

    [Test]
    public async Task UpdateGroupAsync_Success()
    {
        // Arrange
        var groupService = ResolveService<IGroupService>();
        var request = new UpdateGroupRequest()
        {
            Name = "Updated group",
            LeaderId = employeeId
        };

        // Act
        var result = await groupService.UpdateGroupAsync(request, groupId, userId.ToString());
        
        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void UpdateProjectAsync_Failure_GroupIdDoesNotExist()
    {
        // Arrange
        var groupService = ResolveService<IGroupService>();
        var request = new UpdateGroupRequest()
        {
            Name = "Updated group",
            LeaderId = employeeId
        };
        
        // Act
        // Assert
        Assert.ThrowsAsync<GroupDoesNotExistException>(async () =>
            await groupService.UpdateGroupAsync(request, Guid.NewGuid(), userId.ToString()));
    }
    
    [Test]
    public void UpdateProjectAsync_Failure_InvalidUpdaterId()
    {
        // Arrange
        var groupService = ResolveService<IGroupService>();
        var request = new UpdateGroupRequest()
        {
            Name = "Updated group",
            LeaderId = employeeId
        };
        
        // Act
        // Assert
        Assert.ThrowsAsync<InvalidGuidIdException>(async () =>
            await groupService.UpdateGroupAsync(request, groupId, "Invalid guid"));
    }
    
    [Test]
    public void UpdateProjectAsync_Failure_UpdaterDoesNotExist()
    {
        // Arrange
        var groupService = ResolveService<IGroupService>();
        var request = new UpdateGroupRequest()
        {
            Name = "Updated group",
            LeaderId = employeeId
        };
        
        // Act
        // Assert
        Assert.ThrowsAsync<UserDoesNotExistException>(async () =>
            await groupService.UpdateGroupAsync(request, groupId, Guid.NewGuid().ToString()));
    }
    
    [Test]
    public void UpdateProjectAsync_Failure_LeaderDoesNotExist()
    {
        // Arrange
        var groupService = ResolveService<IGroupService>();
        var request = new UpdateGroupRequest()
        {
            Name = "Updated group",
            LeaderId = Guid.NewGuid()
        };
        
        // Act
        // Assert
        Assert.ThrowsAsync<EmployeeDoesNotExistException>(async () =>
            await groupService.UpdateGroupAsync(request, groupId, userId.ToString()));
    }
}