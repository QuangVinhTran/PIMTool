using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Exceptions;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;

namespace PIMTool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService,
            IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("search-employee/{projectId}")]
        public async Task<ActionResult<EmployeeDto>> SearchEmployee(int projectId)
        {
            try
            {
                var employeeList = await _employeeService.SearchEmployeeByProjId(projectId);
                if (employeeList == null)
                {
                    return BadRequest("This project has not been assigned to any employees!");
                }
                return Ok(_mapper.Map<List<EmployeeDto>>(employeeList));
            }
            catch (BusinessException ex)
            {
                // Handle the custom exception and return an appropriate response
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Create(EmployeeDto employeeDto)
        {
            try
            {
                await _employeeService.AddAsync(_mapper.Map<Employee>(employeeDto));
                return Ok("Create an employee successfully");
            }
            catch (BusinessException ex)
            {
                // Handle the custom exception and return an appropriate response
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<EmployeeDto>> Update(EmployeeDto employeeDto)
        {
            try
            {
                var employee = await _employeeService.GetAsync(employeeDto.Id);
                if (employee == null)
                {
                    return BadRequest("Employee does not exist!");
                }
                employeeDto.Version = employee.Version;
                await _employeeService.Update(_mapper.Map<Employee>(employeeDto));
                return Ok("Update employee successfully!");

            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest("This employee has been updated. Please load the page and try again!");
            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


    }
}
