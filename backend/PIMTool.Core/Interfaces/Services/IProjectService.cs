using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Models;
using PIMTool.Core.Models.Request;

namespace PIMTool.Core.Interfaces.Services;

public interface IProjectService
{
    Task<ApiActionResult> GetAllProjectsAsync();
    Task<ApiActionResult> FindProjectsAsync(SearchProjectsRequest searchProjectsRequest);
    Task<ApiActionResult> CreateProjectAsync(CreateProjectRequest createProjectRequest);
    Task<ApiActionResult> CheckIfProjectNumberExistsAsync(int projectNumber);
    Task<ApiActionResult> UpdateProjectAsync(UpdateProjectRequest request, Guid id, string updaterId);
    Task<ApiActionResult> DeleteProjectAsync(Guid id);
    Task<ApiActionResult> FindProjectByProjectNumberAsync(int projectNumber);
    Task<ApiActionResult> DeleteMultipleProjectsAsync(DeleteMultipleProjectsRequest request);
    Task<FileStreamResult> ExportProjectsToFileAsync(ExportProjectsToFileRequest request);
    Task<FileStreamResult> ImportProjectsFromFileNpoiAsync(IFormFile file);
}