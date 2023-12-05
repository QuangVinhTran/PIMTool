using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Services;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Database;
using PIMTool.Dtos;
using System;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Controllers
{
    public class EmployeeController : BaseApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetEmps")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> Get()
        {
            var entities = await _employeeService.GetEmployees();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!entities.Any())
            {
                return NotFound();
            }
            return base.Ok(_mapper.Map<IEnumerable<EmployeeDto>>(entities));
        }

        //search by visa, first name, last name of employee
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> Search([FromQuery] string searchText)
        {
            var entities = await _employeeService.GetEmployees();
            if (searchText != null)
            {
                entities = await _employeeService.Search(searchText);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!entities.Any())
            {
                return NotFound();
            }
            return base.Ok(_mapper.Map<IEnumerable<EmployeeDto>>(entities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> Get([FromRoute][Required] int id)
        {
            var entity = await _employeeService.GetAsync(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (entity == null)
            {
                return NotFound();
            }
            return base.Ok(_mapper.Map<EmployeeDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee([FromBody] EmployeeDto empDto)
        {
            var emp = _mapper.Map<Employee>(empDto);
            await _employeeService.AddAsync(emp);
            var listEmps = await _employeeService.GetEmployees();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return base.CreatedAtRoute("GetEmps", _mapper.Map<IEnumerable<EmployeeDto>>(listEmps));
        }

        [HttpPut]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployee([FromQuery] int id, [FromBody] EmployeeDto empDto)
        {
            if (id != empDto.Id || empDto == null)
            {
                return BadRequest("Invalid employee data or mismatched ID.");
            }

            var currentEmp = await _employeeService.GetAsync(id);
            //var currentEmp = _pimContext.Employees.Find(id);
            currentEmp.FirstName = empDto.FirstName;
            currentEmp.LastName = empDto.LastName;
            currentEmp.BirthDate = empDto.BirthDate;
            currentEmp.Visa = empDto.Visa;

            if (currentEmp == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    await _employeeService.UpdateAsync();
                    //_pimContext.SaveChanges();
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return base.Ok(_mapper.Map<EmployeeDto>(currentEmp));
            //return Ok("update success");
        }

        [HttpDelete]
        public async Task<ActionResult<EmployeeDto>> DeleteEmployee([FromQuery] int id)
        {
            var emp = await _employeeService.GetAsync(id);
            if (emp == null)
            {
                return NotFound();
            }
            await _employeeService.DeleteAsync(emp);
            var listEmps = await _employeeService.GetEmployees();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return base.CreatedAtRoute("GetEmps", _mapper.Map<IEnumerable<EmployeeDto>>(listEmps));
        }

    }
}
