using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;
using PIMTool.Dtos.Group;

namespace PIMTool.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("group")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        IGroupSevice _groupSevice;
        IMapper _mapper;
        ResponseDto _responseDto;

        public GroupController(IGroupSevice groupSevice, IMapper mapper)
        {
            _groupSevice = groupSevice;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }
        [HttpGet("getAllId")]
        public async Task<ResponseDto> GetAllId() {
            try
            {
                List<int> list = new List<int>();
                IEnumerable<Group> listGroup = await _groupSevice.GetAll();
                foreach (Group group in listGroup)
                {
                    list.Add(group.Id);
                }
                _responseDto.Data = list;
            } catch (Exception ex)
            {
                _responseDto.isSuccess = false;
                _responseDto.Error = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        public async Task<ResponseDto> GetAll()
        {
            try
            {
                IEnumerable<GroupDto> groups = _mapper.Map<IEnumerable<GroupDto>>(await _groupSevice.GetAll());
                _responseDto.Data = groups;
            }
            catch
            (Exception ex)
            {
                _responseDto.Error = ex.Message;
                _responseDto.isSuccess = false;
            }
            return _responseDto;
        }
        [HttpPost]
        public async Task<ResponseDto> Create([FromBody] GroupDto groupDto)
        {
            try
            {
                Group group = _mapper.Map<Group>(groupDto);
                await _groupSevice.Create(group);
                _responseDto.Data = groupDto;
            } catch (Exception ex)
            {
                _responseDto.isSuccess=false;
                _responseDto.Error = ex.Message;
            }
            return _responseDto;
        }
    }
}
