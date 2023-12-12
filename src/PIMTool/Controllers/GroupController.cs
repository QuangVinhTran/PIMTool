using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;

namespace PIMTool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GroupDto>> Get()
        {
            var groupList = _groupService.GetAll().ToList();
            return Ok(_mapper.Map<List<GroupDto>>(groupList));
        }
    }
}
