using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Domain.Objects;
using PIMTool.Core.Exceptions;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;

namespace PIMTool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IProjectEmployeeService _projectEmployeeService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService,
            IMapper mapper, IProjectEmployeeService projectEmployeeService)
        {
            _projectService = projectService;
            _projectEmployeeService = projectEmployeeService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> Get([FromRoute][Required] int id)
        {
            var entity = await _projectService.GetAsync(id);
            if(entity == null)
            {
                return BadRequest("Project does not exist!");
            }
            return Ok(_mapper.Map<ProjectDto>(entity));
        }

        [HttpGet]
        public async Task<ActionResult<ProjectDto>> GetAll([FromQuery]ProjectParameters projectParameters)
        {
            var projectList = _projectService.Get(projectParameters).ToList();
            return Ok(_mapper.Map<List<ProjectDto>>(projectList));
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create(ProjectDto projectDto)
        {
            try
            {
                await _projectService.AddAsync(_mapper.Map<Project>(projectDto), projectDto.EmployeeVisas);
                return Ok("Create a project successfully");
            }
            catch (BusinessException ex)
            {
                // Handle the custom exception and return an appropriate response
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<ProjectDto>> Update(ProjectDto projectDto)
        {
            try
            {
                var project = await _projectService.GetAsync(projectDto.Id);
                if (project == null)
                {
                    return BadRequest("Project does not exist!");
                }
                projectDto.Version = project.Version;
                await _projectService.Update(_mapper.Map<Project>(projectDto), projectDto.EmployeeVisas);
                return Ok("Update a project successfully!");

            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ProjectDto>> Delete(int id)
        {
            try
            {
                var project = await _projectService.GetAsync(id);
                if (project == null)
                {
                    return BadRequest("Project does not exsit!");
                }
                await _projectService.Delete(project);
                return Ok("Delete a project successfully");
            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}