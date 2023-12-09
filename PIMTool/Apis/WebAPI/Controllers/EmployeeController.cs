using Application.Interfaces.Services;
using Application.ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeViewModel createEmployeeViewModel)
        {
            var createdEmployee = await _employeeService.CreateEmployee(createEmployeeViewModel);

            var createdEmployeeData = createdEmployee.Data;

            var responseData = new
            {
                message = "Employee created successfully",
                data = createdEmployeeData
            };

            return CreatedAtAction(nameof(GetEmployeeById), new { createdEmployeeData.Id }, responseData);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var deletedUser = await _employeeService.DeleteEmployee(id);

            return Ok(deletedUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployee()
        {
            var Employees = await _employeeService.GetEmployees();
            return Ok(Employees);
        }

        [HttpGet("/api/Employee/get-employees-by-visa/{visa}")]
        public async Task<IActionResult> SearchEmployeesByVisa(string visa, CancellationToken cancellationToken = default)
        {
            var result = await _employeeService.SearchEmployeeByVisaAsync(visa, cancellationToken);

            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var Employee = await _employeeService.GetEmployeeById(id);

            return Ok(Employee);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateEmployeeById(int id, UpdateEmployeeViewModel model)
        {
            var result = await _employeeService.UpdateEmployee(id, model);
            return Ok(result);
        }
    }
}