using Application.Interfaces.Services;
using Application.ViewModels.GroupViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class GroupController : BaseController
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupViewModel model)
        {
            var created = await _groupService.CreateGroup(model);

            if (created.Success)
            {
                var createdData = created.Data;

                var responseData = new
                {
                    message = "Group created successfully",
                    data = createdData
                };

                return CreatedAtAction(nameof(GetGroupById), new { createdData.Id }, responseData);

            }
            else
            {
                return BadRequest(new { message = created.Message, errors = created.ErrorMessages });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var deleted = await _groupService.DeleteGroup(id);
            if (!deleted.Success)
            {
                return NotFound(deleted);
            }
            return Ok(deleted);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllGroup()
        {
            var groups = await _groupService.GetAllGroup();
            if (!groups.Success) return NotFound(groups);
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(int id)
        {
            var group = await _groupService.GetGroupById(id);

            if (!group.Success)
            {
                return NotFound(group);
            }
            return Ok(group);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateGroupById(int id, UpdateGroupViewModel model)
        {
            var result = await _groupService.UpdateGroup(id, model);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            else if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }
    }
}