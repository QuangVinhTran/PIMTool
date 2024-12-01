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
            try
            {
                var entity = await _projectService.GetAsync(id);
                if (entity == null)
                {
                    return BadRequest(_mapper.Map<ProjectDto>(entity));
                }
                ProjectDto result = _mapper.Map<ProjectDto>(entity);
                foreach (var employee in entity.Employees)
                {
                    result.EmployeeVisas.Add(employee.Visa);
                }
                return Ok(result);

            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }

        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectDto>>> GetAll([FromQuery] ProjectParameters projectParameters)
        {
            try
            {
                var projectList = _projectService.Get(projectParameters).ToList();
                var projectDtoList = _mapper.Map<List<ProjectDto>>(projectList);
                foreach (var projectDto in projectDtoList)
                    projectDto.TotalPage = projectParameters.PagingParameters.TotalPage;
                return Ok(projectDtoList);
            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }

        }

        [HttpPost]
        public async Task<ActionResult> Create(ProjectDto projectDto)
        {
            try
            {
                await _projectService.AddAsync(_mapper.Map<Project>(projectDto), projectDto.EmployeeVisas);
                return Ok();
            }
            catch (BusinessException ex)
            {
                // Handle the custom exception and return an appropriate response
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(ProjectDto projectDto)
        {
            try
            {
                var project = await _projectService.GetAsync(projectDto.Id);
                if (project == null)
                {
                    return BadRequest();
                }
                await _projectService.Update(_mapper.Map<Project>(projectDto), projectDto.EmployeeVisas);
                return Ok();

            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var project = await _projectService.GetAsync(id);
                if (project == null)
                {
                    return BadRequest();
                }
                await _projectService.Delete(project);
                return Ok();
            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAll([FromBody] List<int> idList)
        {
            try
            {
                List<Project> projects = new();
                foreach (var id in idList)
                {
                    var project = await _projectService.GetAsync(id);
                    if (project != null)
                        projects.Add(project);
                }
                if (projects.Count() == 0)
                {
                    return BadRequest();
                }
                await _projectService.DeleteRange(projects.ToArray());
                return Ok();
            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("Number")]
        public async Task<ActionResult<ProjectDto>> SearchByProjectNumber([FromQuery] int number)
        {
            try
            {
                var project = _projectService.SearchProjectByNumber(number).FirstOrDefault();
                return Ok(project);

            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}