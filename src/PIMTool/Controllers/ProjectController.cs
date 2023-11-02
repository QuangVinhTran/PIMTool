using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;
using PIMTool.Services;

namespace PIMTool.Controllers
{
    public class ProjectController : BaseApiController
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService,
            IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
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

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] ProjectDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            await _projectService.AddAsync(project);
            var listProjects = await _projectService.GetProjects();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return base.CreatedAtRoute("GetProjects", _mapper.Map<IEnumerable<ProjectDto>>(listProjects));
        }

        [HttpPut]
        public async Task<ActionResult<ProjectDto>> UpdateProject([FromQuery][Required] int id,
                       [FromBody] ProjectDto projectDto)
        {
            if (id != projectDto.Id || projectDto == null)
            {
                return BadRequest("Invalid project data or mismatched ID.");
            }

            var currentProject = await _projectService.GetAsync(id);
            currentProject.Name = projectDto.Name;
            currentProject.Customer = projectDto.Customer;
            currentProject.Status = projectDto.Status;
            currentProject.StartDate = projectDto.StartDate;
            currentProject.EndDate = projectDto.EndDate;
            currentProject.GroupId = projectDto.GroupId;

            if (currentProject == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    await _projectService.UpdateAsync();
                    return Ok(_mapper.Map<ProjectDto>(currentProject));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Employee)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];

                                // TODO: decide which value should be written to database
                                // proposedValues[property] = <value to be saved>;
                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }
            }
            return Ok(_mapper.Map<ProjectDto>(currentProject));
        }

        [HttpDelete]
        public async Task<ActionResult<ProjectDto>> DeleteProject([FromQuery][Required] int id)
        {
            var project = await _projectService.GetAsync(id);
            await _projectService.DeleteAsync(project);
            return Ok(project);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> Get([FromRoute][Required] int id)
        {
            var entity = await _projectService.GetAsync(id);
            return Ok(_mapper.Map<ProjectDto>(entity));
        }
    }
}