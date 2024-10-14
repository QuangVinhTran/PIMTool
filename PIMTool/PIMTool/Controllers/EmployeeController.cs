using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Entities;
using PIMTool.Payload.Request.Paging;
using PIMTool.Payload.Request.Service;
using PIMTool.Payload.Response;
using PIMTool.Services;

namespace PIMTool.Controllers;



[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IMapper mapper, IEmployeeService employeeService)
    {
        _mapper = mapper;
        _employeeService = employeeService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllEmployee()
    {
        var employees = await _employeeService.GetAllEmployees();
        if (!employees.Any())
        {
            return NotFound(new BaseResponse("Not Found!", null));
        }
        return Ok(new BaseResponse("Successful!", _mapper.Map<List<EmployeeResponse>>(employees.OrderBy(emp => emp.FirstName))));
    }
    
    [HttpGet("get-all-with-paging/{currentPage}/{pageSize}")]
    public async Task<IActionResult> GetAllEmployeesWithPaging(int currentPage, int pageSize)
    {
        var employees = await _employeeService.GetAllEmployeesWithPaging(new PagingParameter(currentPage, pageSize));
        if (!employees.Any())
        {
            return NotFound(new BaseResponse("Not Found!", null));
        }
        return Ok(new BaseResponse("Successful!", _mapper.Map<List<EmployeeResponse>>(employees.OrderBy(emp => emp.FirstName))));
    }

    [HttpPost("insert")]
    public async Task<IActionResult> InsertNewEmployee(CEmployeeRequest request)
    {
        try
        {
            await _employeeService.AddNewEmployee(_mapper.Map<CEmployeeRequest, EmployeeEntity>(request));
            return Created("", new BaseResponse("Add successful!", null));
        }
        catch (Exception ex)
        {
            var employee = _mapper.Map<CEmployeeRequest, EmployeeEntity>(request);
            employee.Version = 0;
            return BadRequest(new BaseResponse("", employee));
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteEmployee(DEmployeeRequest request)
    {
        try
        {
            await _employeeService.DeleteEmployee(_mapper.Map<DEmployeeRequest, EmployeeEntity>(request));
            return Ok(new BaseResponse("Delete successfully!", null));
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse("Delete unsuccessfully!", ex.Message));
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> ModifyEmployee(UEmployeeRequest request)
    {
        try
        {
            var updateEmployee = _mapper.Map<UEmployeeRequest, EmployeeEntity>(request);
            await _employeeService.UpdateEmployee(updateEmployee);
            return Ok(new BaseResponse("Successfully updated!", null!));
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse("Unable to save because the entity was updated by another user!", ex.Message));
        }
    }
}