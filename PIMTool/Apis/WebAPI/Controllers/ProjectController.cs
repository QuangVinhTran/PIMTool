using System.Threading;
using Application.Interfaces.Repositories;
using Application.ViewModels.ProjectViewModels;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectViewModel model, CancellationToken cancellationToken = default)
        {
            // Sử dụng cancellationToken để kiểm tra hủy yêu cầu
            if (cancellationToken.IsCancellationRequested)
            {
                System.Console.WriteLine("CancellationToken is called");
                return NoContent();
            }

            var created = await _projectService.CreateProject(model, cancellationToken);

            if (!created.Success)
            {
                return BadRequest(new { error = created.ErrorMessages });
            }

            var createdData = created.Data;

            var responseData = new
            {
                message = "Project created successfully",
                data = createdData
            };

            return CreatedAtAction(nameof(GetProjectById), new { id = createdData.Id }, responseData);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id, CancellationToken cancellationToken = default)
        {
            // Sử dụng cancellationToken để kiểm tra hủy yêu cầu
            if (cancellationToken.IsCancellationRequested)
            {
                System.Console.WriteLine("CancellationToken is called");
                return NoContent();
            }

            var deleted = await _projectService.DeleteProject(id, cancellationToken);

            if (deleted.Success)
            {
                return Ok(deleted);
            }
            else
            {
                return BadRequest(new { error = deleted.ErrorMessages });
            }
        }

        [HttpDelete("/api/projects/delete-projects")]
        public async Task<IActionResult> DeleteProjects([FromBody] int[] id, CancellationToken cancellationToken = default)
        {
            // Sử dụng cancellationToken để kiểm tra hủy yêu cầu
            if (cancellationToken.IsCancellationRequested)
            {
                System.Console.WriteLine("CancellationToken is called");
                return NoContent();
            }

            var deleted = await _projectService.DeleteProjects(id, cancellationToken);

            if (deleted.Success)
            {
                return Ok(deleted);
            }
            else
            {
                return BadRequest(new { error = deleted.ErrorMessages });
            }
        }

        [HttpGet("/api/projects/isExist")]
        public IActionResult IsProjectNumberExisted(int projectNumber)
        {
            var result = _projectService.IsProjectNumberExists(projectNumber);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProject(int pageIndex = 0, int pageSize = 5, CancellationToken cancellationToken = default)
        {
            // Sử dụng cancellationToken để kiểm tra hủy yêu cầu
            if (cancellationToken.IsCancellationRequested)
            {
                System.Console.WriteLine("CancellationToken is called");
                return NoContent();
            }
            var projects = await _projectService.GetProjectPagingAsync(pageIndex, pageSize, cancellationToken);
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id, CancellationToken cancellationToken = default)
        {
            // Sử dụng cancellationToken để kiểm tra hủy yêu cầu
            if (cancellationToken.IsCancellationRequested)
            {
                System.Console.WriteLine("CancellationToken is called");
                return NoContent();
            }
            var result = await _projectService.GetProjectById(id, cancellationToken);

            return Ok(result);
        }

        [HttpGet("/api/projects/search")]
        public async Task<IActionResult> SearchProject(string? searchTerm, int pageIndex = 0, int pageSize = 5)
        {
            if (string.IsNullOrEmpty(searchTerm)) return await GetAllProject(pageIndex, pageSize);
            var projects = await _projectService.SearchProjectAsync(searchTerm, pageIndex, pageSize);
            return Ok(projects);
        }

        [HttpGet("/api/projects/filter")]
        public async Task<IActionResult> FilterProjects(string? searchTerm, StatusEnum? status, int pageIndex = 0, int pageSize = 5, CancellationToken cancellationToken = default)
        {
            // Sử dụng cancellationToken để kiểm tra hủy yêu cầu
            if (cancellationToken.IsCancellationRequested)
            {
                System.Console.WriteLine("CancellationToken is called");
                return NoContent();
            }

            if (string.IsNullOrEmpty(searchTerm) && !status.HasValue) return await GetAllProject(pageIndex, pageSize, cancellationToken);

            var projects = await _projectService.FilterProjectsAsync(searchTerm, status, pageIndex, pageSize, cancellationToken);

            return Ok(projects);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectById(int id, UpdateProjectViewModel model, CancellationToken cancellationToken = default)
        {
            // Sử dụng cancellationToken để kiểm tra hủy yêu cầu
            if (cancellationToken.IsCancellationRequested)
            {
                System.Console.WriteLine("CancellationToken is called");
                return NoContent();
            }

            var result = await _projectService.UpdateProject(id, model, cancellationToken);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new { error = result.ErrorMessages });
            }

        }

        [HttpGet("/api/projects/{id}/employees")]
        public async Task<IActionResult> GetEmployeesInProject(int id, CancellationToken cancellationToken = default)
        {
            // Sử dụng cancellationToken để kiểm tra hủy yêu cầu
            if (cancellationToken.IsCancellationRequested)
            {
                System.Console.WriteLine("CancellationToken is called");
                return NoContent();
            }

            var result = await _projectService.GetEmployeeInProject(id, cancellationToken);

            return Ok(result);
        }

        [HttpGet("/api/projects/filter-status")]
        public async Task<IActionResult> FilterProjectByStatus(StatusEnum status, int pageIndex = 0, int pageSize = 5)
        {
            var result = await _projectService.GetProjectsByStatus(status, pageIndex, pageSize);
            return Ok(result);
        }
    }
}