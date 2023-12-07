using PIMTool.Core.Exceptions;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Core.Models;
using PIMTool.Core.Models.Request;

namespace PIMTool.Test;

public class EmployeeServiceTest : BaseTest
{
    [Test]
    public async Task GetAllEmployee_Success()
    {
        // Arrange
        var employeeService = ResolveService<IEmployeeService>();
        
        // Act
        var result = await employeeService.GetAllEmployeesAsync();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null.And.Not.Empty);
        });
    }

    [Test]
    public async Task CreateEmployeeAsync_Success()
    {
        // Arrange
        var employeeService = ResolveService<IEmployeeService>();
        var request = new CreateEmployeeRequest()
        {
            Visa = "MOCK",
            FirstName = "Mock",
            LastName = "Employee",
            BirthDate = DateTime.Now
        };
        
        // Act
        var result = await employeeService.CreateEmployeeAsync(request);
        
        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
    
    [Test]
    public async Task DeleteEmployeeAsync_Success()
    {
        // Arrange
        var employeeService = ResolveService<IEmployeeService>();
        
        // Act
        var result = await employeeService.DeleteEmployeeAsync(employeeId);
        
        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
    
    [Test]
    public void DeleteEmployeeAsync_Failure_EmployeeDoesNotExist()
    {
        // Arrange
        var employeeService = ResolveService<IEmployeeService>();
        
        // Act
        // Assert
        Assert.ThrowsAsync<EmployeeDoesNotExistException>(async () => await employeeService.DeleteEmployeeAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task FindEmployeeAsync_Success()
    {
        // Arrange
        var employeeService = ResolveService<IEmployeeService>();
        
        // Act
        var result = await employeeService.FindEmployeeAsync(employeeId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        });
    }
    
    [Test]
    public void FindEmployeeAsync_Failure_EmployeeDoesNotExist()
    {
        // Arrange
        var employeeService = ResolveService<IEmployeeService>();
        
        // Act
        // Assert
        Assert.ThrowsAsync<EmployeeDoesNotExistException>(async () =>
            await employeeService.FindEmployeeAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task UpdateEmployeeAsync_Success()
    {
        // Arrange
        var employeeService = ResolveService<IEmployeeService>();
        var request = new UpdateEmployeeRequest()
        {
            FirstName = "Updated first name",
            LastName = "Updated last name",
        };
        
        // Act
        var result = await employeeService.UpdateEmployeeAsync(request, employeeId, userId.ToString());
        
        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
    
    [Test]
    public void UpdateEmployeeAsync_Failure_EmployeeDoesNotExist()
    {
        // Arrange
        var employeeService = ResolveService<IEmployeeService>();
        var request = new UpdateEmployeeRequest()
        {
            FirstName = "Updated first name",
            LastName = "Updated last name",
        };
        
        // Act
        // Assert
        Assert.ThrowsAsync<EmployeeDoesNotExistException>(async () =>
            await employeeService.UpdateEmployeeAsync(request, Guid.NewGuid(), userId.ToString()));
    }
    
    [Test]
    public void UpdateEmployeeAsync_Failure_InvalidUpdaterId()
    {
        // Arrange
        var employeeService = ResolveService<IEmployeeService>();
        var request = new UpdateEmployeeRequest()
        {
            FirstName = "Updated first name",
            LastName = "Updated last name",
        };
        
        // Act
        // Assert
        Assert.ThrowsAsync<InvalidGuidIdException>(async () =>
            await employeeService.UpdateEmployeeAsync(request, employeeId, "Invalid guid"));
    }
    
    [Test]
    public void UpdateEmployeeAsync_Failure_UpdaterIdDoesExist()
    {
        // Arrange
        var employeeService = ResolveService<IEmployeeService>();
        var request = new UpdateEmployeeRequest()
        {
            FirstName = "Updated first name",
            LastName = "Updated last name",
        };
        
        // Act
        // Assert
        Assert.ThrowsAsync<UserDoesNotExistException>(async () =>
            await employeeService.UpdateEmployeeAsync(request, employeeId, Guid.NewGuid().ToString()));
    }

    [Test]
    public async Task FindEmployeesAsync_Success()
    {
        // Arrange
        var employeeService = ResolveService<IEmployeeService>();
        var request = new SearchEmployeesRequest()
        {
            PageIndex = 1,
            PageSize = 10,
            SearchCriteria = new SearchCriteria
            {
                ConjunctionSearchInfos = new List<SearchByInfo>
                {
                    new ()
                    {
                        FieldName = "firstName",
                        Value = "mock"
                    }
                },
                DisjunctionSearchInfos = new List<SearchByInfo>
                {
                    new ()
                    {
                        FieldName = "lastName",
                        Value = "emp"
                    }
                }
            },
            SortByInfos = new List<SortByInfo>
            {
                new ()
                {
                    FieldName = "firstName",
                    Ascending = true
                },
                new ()
                {
                    FieldName = "visa",
                    Ascending = true
                }
            }
        };

        // Act
        var result = await employeeService.FindEmployeesAsync(request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        });
    }
}