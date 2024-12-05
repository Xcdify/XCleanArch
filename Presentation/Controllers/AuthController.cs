using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using XMP.Application.DTOs;
using XMP.Application.Interfaces;
using XMP.Domain.Entities;
using static XMP.Application.DTOs.LoginResponseDto;

namespace XMP.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginRequestDto requestDto)
        {
            try
            {
                var request = _mapper.Map<LoginRequest>(requestDto);
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterRequestDto requestDto)
        {
            try
            {
                var request = _mapper.Map<RegisterRequest>(requestDto);
                var result = await _authService.RegisterAsync(request);
                return result ? Ok(new { message = "User registered successfully" }) : BadRequest("Registration failed.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public class TokenResponse
        {
            public string Token { get; set; } = string.Empty;
        }
    }
}
