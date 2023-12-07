using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Controllers.Base;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Core.Models.Request;

namespace PIMTool.Controllers;

[Authorize]
[Route("[controller]")]
public class EmployeesController : BaseController
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [Route("all")]
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        return await ExecuteApiAsync(
            async () => await _employeeService.GetAllEmployeesAsync().ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    
    [HttpPost]
    public async Task<IActionResult> GetEmployees(SearchEmployeesRequest searchEmployeesRequest)
    {
        return await ExecuteApiAsync(
            async () => await _employeeService.FindEmployeesAsync(searchEmployeesRequest).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    
    [Route("{id:guid}")]
    [HttpPost]
    public async Task<IActionResult> GetEmployee(Guid id)
    {
        return await ExecuteApiAsync(
            async () => await _employeeService.FindEmployeeAsync(id).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    
    [Route("create")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployee(CreateEmployeeRequest createEmployeeRequest)
    {
        return await ExecuteApiAsync(
            async () => await _employeeService.CreateEmployeeAsync(createEmployeeRequest).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    
    [Route("update/{id:guid}")]
    [HttpPut]
    public async Task<IActionResult> UpdateEmployee(UpdateEmployeeRequest request, Guid id)
    {
        var updaterId = HttpContext.Request.Headers["UpdaterId"].ToString();
        return await ExecuteApiAsync(
            async () => await _employeeService.UpdateEmployeeAsync(request, id, updaterId).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    
    [Route("delete/{id:guid}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEmployee(Guid id)
    {
        return await ExecuteApiAsync(
            async () => await _employeeService.DeleteEmployeeAsync(id).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
}