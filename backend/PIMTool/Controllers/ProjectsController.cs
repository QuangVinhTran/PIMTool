using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Controllers.Base;
using PIMTool.Core.Attributes;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Core.Models.Request;

namespace PIMTool.Controllers;

[Authorize]
[Route("[controller]")]
public class ProjectsController : BaseController
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAllProjects()
    {
        return await ExecuteApiAsync(
            async () => await _projectService.GetAllProjectsAsync().ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [HttpPost]
    public async Task<IActionResult> GetProjects(SearchProjectsRequest searchProjectsRequest)
    {
        return await ExecuteApiAsync(
            async () => await _projectService.FindProjectsAsync(searchProjectsRequest).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    
    [Route("{projectNumber:int}")]
    [HttpPost]
    public async Task<IActionResult> GetProject(int projectNumber)
    {
        return await ExecuteApiAsync(
            async () => await _projectService.FindProjectByProjectNumberAsync(projectNumber).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [Route("create")]
    [HttpPost]
    public async Task<IActionResult> CreateProject(CreateProjectRequest createProjectRequest)
    {
        return await ExecuteApiAsync(
            async () => await _projectService.CreateProjectAsync(createProjectRequest).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [Route("validate/{projectNumber:int}")]
    [HttpPost]
    public async Task<IActionResult> ValidateProjectNumber(int projectNumber)
    {
        return await ExecuteApiAsync(
            async () => await _projectService.CheckIfProjectNumberExistsAsync(projectNumber).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [Route("update/{id:guid}")]
    [HttpPut]
    public async Task<IActionResult> UpdateProject(UpdateProjectRequest updateProjectRequest, Guid id)
    {
        var updaterId = HttpContext.Request.Headers["UpdaterId"].ToString();
        return await ExecuteApiAsync(
            async () => await _projectService.UpdateProjectAsync(updateProjectRequest, id, updaterId).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    
    [Route("delete/{id:guid}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        return await ExecuteApiAsync(
            async () => await _projectService.DeleteProjectAsync(id).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [Route("delete")]
    [HttpPost]
    public async Task<IActionResult> DeleteMultipleProjects(DeleteMultipleProjectsRequest request)
    {
        return await ExecuteApiAsync(
            async () => await _projectService.DeleteMultipleProjectsAsync(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [Route("import-from-file")]
    [HttpPost]
    public async Task<IActionResult> ImportProjectsFromFile([AcceptFileExtensions(".csv,.xlsx")] IFormFile file)
    {
        return await _projectService.ImportProjectsFromFileNpoiAsync(file);

        // return await ExecuteApiAsync(
        //     async () => await _projectService.ImportProjectsFromFileNpoiAsync(file).ConfigureAwait(false)
        // ).ConfigureAwait(false);
    }
    
    [Route("export-to-excel")]
    [HttpPost]
    public async Task<IActionResult> ExportProjectsToFile(ExportProjectsToFileRequest request)
    {
        return await _projectService.ExportProjectsToFileAsync(request);
        // return await ExecuteApiAsync(
        //     async () => await _projectService.ExportProjectsToFileAsync().ConfigureAwait(false)
        // ).ConfigureAwait(false);
    }
}