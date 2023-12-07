using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;
using System.Diagnostics;

namespace PIMTool.Controllers
{
    [Route("group")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public GroupController(IGroupService groupService, IEmployeeService employeeService,
            IMapper mapper)
        {
            _groupService = groupService;
            _employeeService = employeeService; 
            _mapper = mapper;
        }


        [HttpPost()]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequestDto request)
        {
            Employee leader = await _employeeService.GetAsync(request.LeaderId);
            List<Employee> employees = new List<Employee>();
            employees.Add(leader);
            foreach (int id in request.EmployeeIds)
            {
                Employee employee = await _employeeService.GetAsync(id);
                employees.Add(employee);
            }
            Group group = new Group
            {
                Version = request.Version,
                GroupLeaderId = leader.Id,
                Employees = employees
            };
            await _groupService.AddAsync(group);

            return Ok(group);
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<List<GroupDto>>> GetAllGroup()
        {
            var groups = Task.Run(() => _groupService.Get().Select(g => new GroupDto
            {
                Id = g.Id,
                GroupLeaderId = g.GroupLeaderId,
                Version = g.Version,
                Employees = g.Employees.Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    BirthDate = e.BirthDate,
                    Visa = e.Visa,
                    Version = e.Version
                }).ToList()
            })).Result;

            return Ok(groups);
        }
    }
}
