using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;
using PIMTool.Services;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Controllers
{
    public class ProjectEmployeeController : BaseApiController
    {
        private readonly IProjectEmployeeService _projectEmployeeService;
        private readonly IMapper _mapper;

        public ProjectEmployeeController(IProjectEmployeeService projectEmployeeService, IMapper mapper)
        {
            _projectEmployeeService = projectEmployeeService;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetProjectEmployees")]
        public async Task<ActionResult<IEnumerable<ProjectEmployeeDto>>> Get()
        {
            var entities = await _projectEmployeeService.GetProjectEmployees();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!entities.Any())
            {
                return NotFound();
            }
            return base.Ok(_mapper.Map<IEnumerable<ProjectEmployeeDto>>(entities));
        }

        [HttpGet("{projectId}/{employeeId}")]
        public async Task<ActionResult<ProjectEmployeeDto>> Get([FromRoute][Required] int projectId, [FromRoute][Required] int employeeId)
        {
            var entity = await _projectEmployeeService.GetAsync(employeeId, projectId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (entity == null)
            {
                return NotFound();
            }
            return base.Ok(_mapper.Map<ProjectEmployeeDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<ProjectEmployeeDto>> CreateProjectEmployee([FromBody] ProjectEmployeeDto projectEmployeeDto)
        {
            var projectEmployee = _mapper.Map<ProjectEmployee>(projectEmployeeDto);
            await _projectEmployeeService.AddAsync(projectEmployee);
            var listProjectEmployees = await _projectEmployeeService.GetProjectEmployees();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return base.CreatedAtRoute("GetProjectEmployees", _mapper.Map<IEnumerable<ProjectEmployeeDto>>(listProjectEmployees));
        }

        //[HttpPut]
        //public async Task<ActionResult<EmployeeDto>> Update([FromQuery] int id, [FromBody] ProjectEmployeeDto dto)
        //{
        //    if (id != dto.Id || dto == null)
        //    {
        //        return BadRequest("Invalid project employee data or mismatched ID.");
        //    }

        //    var curPE = await _projectEmployeeService.GetAsync(id);
        //    //var currentEmp = _pimContext.Employees.Find(id);
        //    curPE.ProjectId = dto.ProjectId;
        //    curPE.EmployeeId = dto.EmployeeId;

        //    if (curPE == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        try
        //        {
        //            await _projectEmployeeService.UpdateAsync();
        //            //_pimContext.SaveChanges();
        //        }
        //        catch (DbUpdateConcurrencyException ex)
        //        {
        //            foreach (var entry in ex.Entries)
        //            {
        //                if (entry.Entity is Employee)
        //                {
        //                    var proposedValues = entry.CurrentValues;
        //                    var databaseValues = entry.GetDatabaseValues();

        //                    foreach (var property in proposedValues.Properties)
        //                    {
        //                        var proposedValue = proposedValues[property];
        //                        var databaseValue = databaseValues[property];

        //                        // TODO: decide which value should be written to database
        //                        // proposedValues[property] = <value to be saved>;
        //                    }

        //                    // Refresh original values to bypass next concurrency check
        //                    entry.OriginalValues.SetValues(databaseValues);
        //                }
        //                else
        //                {
        //                    throw new NotSupportedException(
        //                        "Don't know how to handle concurrency conflicts for "
        //                        + entry.Metadata.Name);
        //                }
        //            }
        //        }
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    return base.Ok(_mapper.Map<ProjectEmployeeDto>(curPE));
        //    //return Ok("update success");
        //}

        [HttpDelete]
        public async Task<ActionResult<ProjectEmployeeDto>> DeleteProjectEmployee([FromQuery][Required] int projectId, [FromQuery][Required] int employeeId)
        {
            var projectEmployee = await _projectEmployeeService.GetAsync(employeeId, projectId);
            if (projectEmployee == null)
            {
                return NotFound();
            }
            await _projectEmployeeService.DeleteAsync(projectEmployee);
            var listProjectEmployees = await _projectEmployeeService.GetProjectEmployees();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return base.CreatedAtRoute("GetProjectEmployees", _mapper.Map<IEnumerable<ProjectEmployeeDto>>(listProjectEmployees));
        }
    }
}
