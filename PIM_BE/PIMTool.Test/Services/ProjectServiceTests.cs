using Microsoft.Extensions.DependencyInjection;
using PIMTool.Core.Constants;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Domain.Objects;
using PIMTool.Core.Interfaces.Services;
using System.Runtime.CompilerServices;
using Assert = NUnit.Framework.Assert;

namespace PIMTool.Test.Services;

public class ProjectServiceTests : BaseTest
{
    private IProjectService _projectService = null!;

    [SetUp]
    public void SetUp()
    {
        _projectService = ServiceProvider.GetRequiredService<IProjectService>();
        InitData();
    }
    private void InitData()
    {
        var _context = Context.Set<Project>();
        for (int i = 1; i <= 5; i++)
        {
            var project = new Project();
            project.Name = $"Test {i}";
            project.Customer = $"Test {i}";
            project.ProjectNumber = i;
            project.Status = ProjectStatusConstants.NEW;
            _context.Add(project);
            Context.SaveChanges();
        }
    }
    [Test]
    public async Task GetAllAsync_Success()
    {
        var result = await _projectService.GetAllAsync();
        Assert.IsNotNull(result);
    }
    [Test]
    [TestCase(1)]
    [TestCase(2)]
    public async Task GetAsync_Succes(int id)
    {
        var result = await _projectService.GetAsync(id);
        Assert.IsNotNull(result);
    }
    [Test]
    [TestCase(10)]
    public async Task GetAsync_Fail(int id)
    {
        var result = await _projectService.GetAsync(id);
        Assert.IsNull(result);
    }

    [Test]
    [TestCase(1)]
    public async Task Update_Success(int id)
    {
        var project = await _projectService.GetAsync(id);
        project.Name = "Update";

        await _projectService.Update(project);
        var result = await _projectService.GetAsync(project.Id);
        Assert.AreEqual(result.Name, "Update");
    }

    [Test]
    [TestCase(20)]
    public async Task Update_Fail_NotFound(int id)
    {
        var project = new Project();
        project.Id = id;
        project.Name = "";
        project.Customer = "";
        project.Status = ProjectStatusConstants.NEW;
        try
        {
            await _projectService.Update(project);
            Assert.Fail();
        } catch (Exception ex)
        {
            Assert.Throws<Exception>(() => throw new Exception($"Not found projectId {project.Id}"));
        }
    }
    [Test]
    [TestCase(10)]
    public async Task Create_Success(int projectNumber)
    {
        var project = new Project();
        project.Name = "";
        project.ProjectNumber = projectNumber;
        project.Customer = "";
        project.Status = ProjectStatusConstants.NEW;
        await _projectService.Create(project);
        var result = await _projectService.GetAsync(project.Id);
        Assert.IsNotNull(result);
    }
    [Test]
    [TestCase(1)]
    public async Task Create_Fail_Duplicate_ProjectNumber(int projectNumber)
    {
        var project = new Project();
        project.Name = "";
        project.ProjectNumber = projectNumber;
        project.Customer = "";
        project.Status = ProjectStatusConstants.NEW;
        try
        {
            await _projectService.Create(project);
            Assert.Fail();
        }
        catch (Exception ex)
        {
            Assert.Throws<Exception>(() => throw new Exception("Duplicate project number!"));
        }
    }
    [Test]
    [TestCase(1)]
    public async Task Delete_Success(int id)
    {
        await _projectService.Delete(id);
        var result = await _projectService.GetAsync(id);
        Assert.IsNull(result);
    }
    [Test]
    [TestCase(10)]
    public async Task Delete_Fail_NotFound(int id)
    {
        try
        {
            await _projectService.Delete(id);
            Assert.Fail();
        } catch (Exception)
        {
            Assert.Throws<Exception>(() => throw new Exception($"Not found projectId {id}"));
        }
    }

    //int pageSize, int pageIndex, string? searchText, string searchStatus, string sortNumber, string sortName, string sortStatus, string sortCustomer, string sortStartDate
    [Test]
    [TestCase(10, 1, "", "0", "0", "0", "0", "0", "0")]
    [TestCase(10, 1, "123", "0", "0", "0", "0", "0", "0")]
    [TestCase(10, 1, "", "NEW", "0", "0", "0", "0", "0")]
    [TestCase(10, 1, "123", "NEW", "0", "0", "0", "0", "0")]
    [TestCase(10, 1, "123", "PLA", "0", "0", "0", "0", "0")]
    [TestCase(10, 1, "", "0", "DES", "0", "0", "0", "0")]
    [TestCase(10, 1, "", "0", "ASC", "0", "0", "0", "0")]
    [TestCase(10, 1, "123", "0", "DES", "0", "0", "0", "0")]
    [TestCase(10, 1, "", "0", "0", "DES", "0", "0", "0")]
    [TestCase(10, 1, "", "0", "0", "ASC", "0", "0", "0")]
    [TestCase(10, 1, "", "0", "0", "0", "DES", "0", "0")]
    [TestCase(10, 1, "", "0", "0", "0", "ASC", "0", "0")]
    [TestCase(10, 1, "", "0", "0", "0", "0", "DES", "0")]
    [TestCase(10, 1, "", "0", "0", "0", "0", "ASC", "0")]
    [TestCase(10, 1, "", "0", "0", "0", "0", "0", "DES")]
    [TestCase(10, 1, "", "0", "0", "0", "0", "0", "ASC")]
    public void SearchProject(int pageSize, int pageIndex, string? searchText, string searchStatus, string sortNumber, string sortName, string sortStatus, string sortCustomer, string sortStartDate)
    {
        PagingDto result = _projectService.SearchProjectV2(pageSize, pageIndex, searchText, searchStatus, sortNumber, sortName, sortStatus, sortCustomer, sortStartDate);
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Data);
    }
    [Test]
    [TestCase(1)]
    public async Task CheckExists_True(int id)
    {
        var check = await _projectService.CheckExist(id);
        Assert.AreEqual(true, check);
    }
    [Test]
    [TestCase(10)]
    public async Task CheckExists_Fail(int id)
    {
        var check = await _projectService.CheckExist(id);
        Assert.AreEqual(false, check);
    }
    [Test]
    public async Task RemoveRange_Success()
    {
        List<int> listRemoveId = new List<int> { 1, 2 };
        await _projectService.RemoveRangeById(listRemoveId);
        var project1 = await _projectService.GetAsync(1);
        Assert.IsNull(project1);
        var project2 = await _projectService.GetAsync(2);
        Assert.IsNull(project2);
    }
    [Test]
    [TestCase(1)]
    public void GetInclude_Success(int id)
    {
        var result = _projectService.GetProjectInclude(id);
        Assert.IsNotNull(result);
    }
    [Test]
    [TestCase(10)]
    public void GetInclude_Fail(int id)
    {
        var result = _projectService.GetProjectInclude(id);
        Assert.IsNull(result);
    }
}