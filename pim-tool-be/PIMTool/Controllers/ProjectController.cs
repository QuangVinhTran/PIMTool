using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Exceptions;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;
using PIMTool.Services;

namespace PIMTool.Controllers
{
    public class ProjectController : BaseApiController
    {
        private readonly IProjectService _projectService;
        private readonly IProjectEmployeeService _projectEmployeeService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectService projectService, IProjectEmployeeService projectEmployeeService,
            IMapper mapper, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _projectEmployeeService = projectEmployeeService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet(Name = "GetProjects")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> Get()
        {
            var entities = await _projectService.GetProjects();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!entities.Any())
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<ProjectDto>>(entities));
        }

        //search project by name or status
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> Search([FromQuery] string searchText, [FromQuery] int? status)
        {
            var entities = await _projectService.Search(searchText, status);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!entities.Any())
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<ProjectDto>>(entities));
        }

        //search project by name or status with pagination
        [HttpGet("search-with-pagination")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> SearchWithPagination([FromQuery] string searchText, [FromQuery] int? status, [FromQuery] int limit, [FromQuery] int skip)
        {
            var entities = await _projectService.SearchWithPagination(searchText, status, skip, limit);
            var totalCount = (await _projectService.Search(searchText, status)).Count();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!entities.Any())
            {
                return NotFound();
            }
            var searchResultResponse = new SearchResultResponse
            {
                results = _mapper.Map<IEnumerable<ProjectDto>>(entities),
                totalCount = totalCount
            };
            return Ok(searchResultResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] ProjectMembersDto projectMembersDto)
        {
            // not allow empty any field --> checked
            // check duplicate project number --> checked
            // visa does not exist in employee table --> checked
            // start date must be less than end date --> checked
            // handle unexpected error

            var listProjects = await _projectService.GetProjects();
            if (listProjects.Any(p => p.ProjectNumber == projectMembersDto.ProjectDto.ProjectNumber))
            {
                throw new ProjectNumberAlreadyExistsException();
            }

            var project = _mapper.Map<Project>(projectMembersDto.ProjectDto);
            try
            {
                var addedProject = await _projectService.AddAsync(project);
                if (projectMembersDto.ListEmpId.Length > 0)
                {
                    foreach (var empId in projectMembersDto.ListEmpId)
                    {
                        var projectEmp = new ProjectEmployee
                        {
                            EmployeeId = empId,
                            ProjectId = addedProject.Id
                        };
                        await _projectEmployeeService.AddAsync(projectEmp);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var listProjectsAfter = await _projectService.GetProjects();
            return base.CreatedAtRoute("GetProjects", _mapper.Map<IEnumerable<ProjectDto>>(listProjectsAfter));
        }

        [HttpPut]
        public async Task<ActionResult<ProjectMembersDto>> UpdateProject([FromQuery][Required] int id,
                       [FromBody] ProjectMembersDto projMember)
        {
            if (id != projMember.ProjectDto.Id || projMember == null)
            {
                return BadRequest("Invalid project data or mismatched ID.");
            }

            var currentProject = await _projectService.GetAsync(id);

            if (currentProject == null)
            {
                  throw new ProjectNotFound();
            }


            if (!currentProject.Version.SequenceEqual(projMember.ProjectDto.Version))
            {
                throw new ConcurrentUpdateException();
            }

            currentProject.Name = projMember.ProjectDto.Name;
            currentProject.Customer = projMember.ProjectDto.Customer;
            currentProject.Status = projMember.ProjectDto.Status;
            currentProject.StartDate = projMember.ProjectDto.StartDate;
            currentProject.EndDate = projMember.ProjectDto.EndDate;
            currentProject.GroupId = projMember.ProjectDto.GroupId;

            if (currentProject == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    await _projectService.UpdateAsync();

                    if (projMember.ListEmpId.Length > 0)
                    {
                        //delete all project employee of this project
                        var listProjectEmp = await _projectEmployeeService.GetProjectEmployees();
                        foreach (var projectEmp in listProjectEmp)
                        {
                            if (projectEmp.ProjectId == id)
                            {
                                await _projectEmployeeService.DeleteAsync(projectEmp);
                            }
                        }

                        //add all again
                        foreach (var empId in projMember.ListEmpId)
                        {
                            var projectEmp = new ProjectEmployee
                            {
                                EmployeeId = empId,
                                ProjectId = id
                            };
                            await _projectEmployeeService.AddAsync(projectEmp);
                        }

                    }

                    //get the project and list member id of its after update
                    var project = await _projectService.GetAsync(id);
                    var projectDto = _mapper.Map<ProjectDto>(project);
                    var projectMembers = await _projectEmployeeService.GetProjectMembers(id);
                    var projectMembersDto = new ProjectMembersDto
                    {
                        ProjectDto = projectDto,
                        ListEmpId = projectMembers
                    };

                    return Ok(projectMembersDto);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw new ConcurrentUpdateException();
                }
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ProjectDto>> DeleteProject([FromQuery][Required] int id)
        {
            var project = await _projectService.GetAsync(id);
            if (project == null)
            {
                throw new ConcurrentUpdateForDeleteCaseException();
            }
            await _projectService.DeleteAsync(project);
            return Ok(_mapper.Map<ProjectDto>(project));
        }

        [HttpGet("{projectNumber}")]
        public async Task<ActionResult<ProjectMembersDto>> GetProjByNumber([FromRoute][Required] int projectNumber)
        {
            var project = await _projectService.GetByProjectNumber(projectNumber);
            var projectDto = _mapper.Map<ProjectDto>(project);
            var projectMembers = await _projectEmployeeService.GetProjectMembers(project.Id);
            var projectMembersDto = new ProjectMembersDto
            {
                ProjectDto = projectDto,
                ListEmpId = projectMembers
            };
            return Ok(projectMembersDto);
        }

        //pagination with skip and limit per page
        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjectsPagination([FromQuery] int limit, [FromQuery] int skip)
        {
            var entities = await _projectService.GetProjectsPagination(skip, limit);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!entities.Any())
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<ProjectDto>>(entities));
        }

        //count projects in database
        [HttpGet("count")]
        public async Task<ActionResult<int>> CountProjects()
        {
            int count = await _projectService.GetProjectsCount();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(count);
        }
    }
}