using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyApi.DTOs;
using MyApi.Models;
using MyApi.Services.Interfaces;

namespace MyApi.Controllers;

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

    /// <summary>
    /// Authenticates a user and returns a JWT token
    /// </summary>
    /// <param name="requestDto">The login request containing username and password</param>
    /// <returns>A JWT token for the authenticated user</returns>
    /// <response code="200">Returns the JWT token</response>
    /// <response code="401">If the credentials are invalid</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequestDto requestDto)
    {
        try
        {
            var request = _mapper.Map<LoginRequest>(requestDto);
            var token = await _authService.LoginAsync(request);
            return Ok(new TokenResponse { Token = token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="requestDto">The registration request containing username, password, and email</param>
    /// <returns>A success message if registration is successful</returns>
    /// <response code="200">If the user is successfully registered</response>
    /// <response code="400">If the username already exists or registration fails</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterRequestDto requestDto)
    {
        try
        {
            var request = _mapper.Map<RegisterRequest>(requestDto);
            var result = await _authService.RegisterAsync(request);
            return result ? Ok(new { message = "User registered successfully" }) : BadRequest();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class TokenResponse
{
    public string Token { get; set; } = string.Empty;
}
