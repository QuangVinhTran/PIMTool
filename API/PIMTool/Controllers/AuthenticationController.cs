using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;
using PIMTool.Services;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Controllers
{
    [ApiController]
    [Route("auth")]
    [EnableCors("apiCorsPolicy")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthenticationService authService,
            IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody][Required] LoginRequestDto loginRequestDto)
        {
            bool checkLogin = await _authService.Login(loginRequestDto.Username, loginRequestDto.Password);
            if (checkLogin)
            {
                LoginResponseDto loginResponseDto = new LoginResponseDto { Name = "Admin"};
                return Ok(new { Message = "Login successfully" });
            }
            return BadRequest(new { Message = "Invalid credentials" });
        }

        [HttpGet("{id}")]
        public IActionResult AS([FromRoute][Required] int id)
        {
            return Ok(new { Id = id});
        }
    }
}
