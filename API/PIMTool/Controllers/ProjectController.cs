using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Exceptions;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;
using PIMTool.Services;

namespace PIMTool.Controllers
{
    [ApiController]
    [Route("project")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IGroupService _groupService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, IGroupService groupService, IEmployeeService employeeService,
            IMapper mapper)
        {
            _projectService = projectService;
            _groupService = groupService;
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> Get([FromRoute][Required] int id)
        {
            var entity = await _projectService.GetAsync(id);
            return Ok(_mapper.Map<ProjectDto>(entity));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequestDto request)
        {
            try
            {
                Project newProject = _mapper.Map<Project>(request);
                foreach (var employeeId in request.Members)
                {
                    Employee employee = await _employeeService.GetAsync(employeeId);
                    if (employee.GroupId != request.GroupId)
                    {
                        throw new BusinessException($"An employee does not belong to group ${request.GroupId}");
                    }
                    newProject.ProjectEmployees.Add(new ProjectEmployee { Employee = employee, Project = newProject });
                }

                await _projectService.AddAsync(newProject);

                return Ok(newProject);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong");
            }

        }

        [HttpPost("auto")]
        public async Task<IActionResult> AutoCreateProject()
        {

            for (int i = 1; i < 100; i++)
            {
                try
                {
                    CreateProjectRequestDto request = new CreateProjectRequestDto
                    {
                        ProjectNumber = i,
                        Name = "Project " + i.ToString(),
                        Customer = "Customer " + i.ToString(),
                        Status = "NEW",
                        GroupId = 1,
                        Members = new List<int> { 1, 2 },
                        StartDate = DateTime.Parse("2023-12-05"),
                        EndDate = DateTime.Parse("2023-12-05")
                    };

                    Group group = await _groupService.GetAsync(request.GroupId);

                    Project project = await _projectService.GetByProjectNumber(request.ProjectNumber);
                    if (project != null)
                    {
                        throw new BusinessException($"Project number already exist");
                    }

                    project = new Project
                    {
                        ProjectNumber = request.ProjectNumber,
                        Name = request.Name,
                        Customer = request.Customer,
                        Group = group,
                        StartDate = request.StartDate,
                        EndDate = request.EndDate,
                        Status = request.Status,
                    };

                    foreach (var employeeId in request.Members)
                    {
                        Employee employee = await _employeeService.GetAsync(employeeId);
                        if (employee.GroupId != request.GroupId)
                        {
                            throw new Exception($"An employee does not belong to group ${request.GroupId}");
                        }
                        project.ProjectEmployees.Add(new ProjectEmployee { Employee = employee, Project = project });
                    }

                    await _projectService.AddAsync(project);
                    await _projectService.SaveChangesAsync();
                }
                catch (BusinessException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Something went wrong");
                }
            }
            return Ok();

        }

        [HttpPut("{projectNumber}")]
        public async Task<IActionResult> UpdateProject(int projectNumber, [FromBody] CreateProjectRequestDto request)
        {
            try
            {
                Project newProject = _mapper.Map<Project>(request);
                foreach (var employeeId in request.Members)
                {
                    Employee employee = await _employeeService.GetAsync(employeeId);
                    if (employee.GroupId != request.GroupId)
                    {
                        throw new BusinessException($"An employee does not belong to the group of the existing project.");
                    }

                    newProject.ProjectEmployees.Add(new ProjectEmployee { Employee = employee, Project = newProject });
                }
                await _projectService.UpdateProject(projectNumber, newProject);


                return Ok();
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var queryableSource = _projectService.Get().Select(p => _mapper.Map<ProjectDto>(p));

            PageList<ProjectDto> projects = PageList<ProjectDto>.ToPagedList(queryableSource, pageNumber, pageSize);
            return Ok(projects);
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<Project>> SearchProjectByNameAndStatus(
            [FromQuery] string? searchValue,
            [FromQuery] string? status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5)
        {
            IQueryable<ProjectDto> queryableSource;

            if (string.IsNullOrEmpty(searchValue) && string.IsNullOrEmpty(status))
            {
                queryableSource = _projectService.Get().Select(p => _mapper.Map<ProjectDto>(p));
            }
            else
            {
                queryableSource = _projectService.SearchProjectByProjectNumberOrNameOrCustomerAndStatus(searchValue, status)
                    .Select(p => _mapper.Map<ProjectDto>(p));
            }

            PageList<ProjectDto> projects = PageList<ProjectDto>.ToPagedList(queryableSource, pageNumber, pageSize);

            return Ok(projects);
        }

        [HttpGet("get-by-project-num/{projectNumber}")]
        public async Task<ActionResult<CreateProjectRequestDto>> GetByProjectNumber(int projectNumber)
        {
            var project = await _projectService.GetByProjectNumber(projectNumber);

            if (project == null)
            {
                return NotFound();
            }

            var projectDto = _mapper.Map<CreateProjectRequestDto>(project);

            return Ok(projectDto);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProjects([FromQuery] int[] projectNumbers)
        {
            try
            {
                if (projectNumbers == null || projectNumbers.Length == 0)
                {
                    return BadRequest("No project IDs provided for deletion.");
                }

                List<Project> projects = new List<Project>();
                foreach (int projectNumber in projectNumbers)
                {
                    var project = await _projectService.GetByProjectNumber(projectNumber);
                    if (project == null)
                    {
                        return NotFound($"Project with number {projectNumber} not found.");
                    }

                    projects.Add(project);
                }

                await _projectService.DeleteProjects(projects.ToArray());
                await _projectService.SaveChangesAsync();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}