using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;
using PIMTool.Dtos.Employee;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Controllers;

[Authorize(Roles = "admin")]
[Route("employee")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;
    private readonly ResponseDto _responseDto;

    public EmployeeController(IEmployeeService employeeService, IMapper mapper)
    {
        _employeeService = employeeService;
        _mapper = mapper;
        _responseDto = new ResponseDto();
    }

    [HttpGet]
    public async Task<ResponseDto> GetAll()
    {
        try
        {
            IEnumerable<Employee> list = await _employeeService.GetAllAsync();
            _responseDto.Data = _mapper.Map<IEnumerable<EmployeeDto>>(list);
        }
        catch (Exception ex)
        {
            _responseDto.Error = ex.Message;
            _responseDto.isSuccess = false;
        }
        return _responseDto;
    }
    [HttpGet("{id}")]
    public async Task<ResponseDto> Get([FromRoute][Required] int id)
    {
        try
        {
            var employee = await _employeeService.GetAsync(id);
            _responseDto.Data = _mapper.Map<EmployeeDto>(employee);
        }
        catch (Exception ex)
        {
            _responseDto.isSuccess = false;
            _responseDto.Error = ex.Message;
        }
        return _responseDto;
    }

    [HttpPost]
    public async Task<ResponseDto> Create([FromBody] EmployeeCreateDto employeeCreateDto)
    {
        try
        {
            var employee = _mapper.Map<Employee>(employeeCreateDto);
            await _employeeService.Create(employee);
            _responseDto.Data = employeeCreateDto;
        }
        catch (Exception ex)
        {
            _responseDto.isSuccess = false;
            _responseDto.Error = ex.Message;
        }
        return _responseDto;
    }
    [HttpPut]
    public async Task<ResponseDto> Update([FromBody] EmloyeeUpdateDto emloyeeUpdateDto)
    {
        try
        {
            var employee = _mapper.Map<Employee>(emloyeeUpdateDto);
            await _employeeService.Update(employee);
            _responseDto.Data = emloyeeUpdateDto;
        }
        catch (Exception ex)
        {
            _responseDto.isSuccess = false;
            _responseDto.Error = ex.Message;
        }
        return _responseDto;
    }
    [HttpDelete("{id}")]
    public async Task<ResponseDto> Delete([FromRoute][Required] int id)
    {
        try
        {
            await _employeeService.Delete(id);
            _responseDto.Data = NoContent();
        }
        catch (Exception ex)
        {
            _responseDto.isSuccess = false;
            _responseDto.Error = ex.Message;
        }
        return _responseDto;
    }
}
