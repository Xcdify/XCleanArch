using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyApi.Configuration;
using MyApi.Models;
using MyApi.Repositories.Interfaces;
using MyApi.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace MyApi.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtConfig _jwtConfig;

    public AuthService(IUserRepository userRepository, IOptions<JwtConfig> jwtConfig)
    {
        _userRepository = userRepository;
        _jwtConfig = jwtConfig.Value;
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user == null || string.IsNullOrEmpty(user.Password) || !VerifyPassword(request.Password, user.Password))
            throw new UnauthorizedAccessException("Invalid username or password");

        return GenerateJwtToken(user);
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser != null)
            throw new InvalidOperationException("Username already exists");

        var user = new User
        {
            Username = request.Username,
            Password = HashPassword(request.Password),
            Email = request.Email
        };

        return await _userRepository.CreateUserAsync(user);
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = _jwtConfig.Issuer,
            Audience = _jwtConfig.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch (ArgumentException)
        {
            // If there's an invalid salt (null or empty), return false
            return false;
        }
    }
}
