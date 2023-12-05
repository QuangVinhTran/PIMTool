using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Controllers
{
    public class GroupController : BaseApiController
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetGroups")]
        public async Task<ActionResult<IEnumerable<GroupDto>>> Get()
        {
            var entities = await _groupService.GetGroups();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!entities.Any())
            {
                return NotFound();
            }
            return base.Ok(_mapper.Map<IEnumerable<GroupDto>>(entities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDto>> Get([FromRoute][Required] int id)
        {
            var entity = await _groupService.GetAsync(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (entity == null)
            {
                return NotFound();
            }
            return base.Ok(_mapper.Map<GroupDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<GroupDto>> CreateGroup([FromBody] GroupDto groupDto)
        {
            var group = _mapper.Map<Group>(groupDto);
            await _groupService.AddAsync(group);
            var listGroups = await _groupService.GetGroups();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return base.CreatedAtRoute("GetGroups", _mapper.Map<IEnumerable<GroupDto>>(listGroups));
        }

        [HttpPut]
        public async Task<ActionResult<GroupDto>> UpdateGroup([FromBody] GroupDto groupDto)
        {
            var group = _mapper.Map<Group>(groupDto);
            await _groupService.UpdateAsync(group);
            var listGroups = await _groupService.GetGroups();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return base.CreatedAtRoute("GetGroups", _mapper.Map<IEnumerable<GroupDto>>(listGroups));
        }

        [HttpDelete]
        public async Task<ActionResult<GroupDto>> DeleteGroup([FromQuery] int id)
        {
            var group = await _groupService.GetAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            await _groupService.DeleteAsync(group);
            var listGroups = await _groupService.GetGroups();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return base.CreatedAtRoute("GetGroups", _mapper.Map<IEnumerable<GroupDto>>(listGroups));
        }
    }
}
