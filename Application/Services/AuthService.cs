using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XMP.Application.Configration;
using XMP.Domain.Entities;
using XMP.Domain.Repositories;
using BCrypt.Net;
using XMP.Application.Interfaces;
using static XMP.Application.DTOs.LoginResponseDto;
using XMP.Application.DTOs;

namespace XMP.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtConfig _jwtConfig;

        public AuthService(IUserRepository userRepository, IOptions<JwtConfig> jwtConfig)
        {
            _userRepository = userRepository;
            _jwtConfig = jwtConfig.Value;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null || string.IsNullOrEmpty(user.Password) || !VerifyPassword(request.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid username or password");

            /* return new LoginResponse
             {
                 Token = GenerateJwtToken(user),
                 User = new UserDto
                 {
                     Id = user.Id,
                     Username = user.Username,
                     Email = user.Email,
                     CreatedAt = user.CreatedAt,
                     UpdatedAt = user.UpdatedAt
                 }
             };*/
            return new LoginResponse
            {
                Token = GenerateJwtToken(user),
                Username = user.Name,
                UserId = user.Id,
                Role = user.Role // Assuming the User entity has a Role property
            };
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Username already exists");

            var user = new User
            {
                Name = request.Username,
                Password = HashPassword(request.Password),
                Email = request.Email,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            return await _userRepository.CreateUserAsync(user);
        }

        private string GenerateJwtToken(User user)
        {
            // Check the key and its length
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);
            if (key.Length < 16)
            {
                throw new ArgumentException("JWT Key must be at least 16 bytes long.");
            }

            // Validate user claims
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Id.ToString()))
            {
                throw new ArgumentException("User claims cannot be null or empty.");
            }

            // Create the token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
        }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _jwtConfig.Issuer ?? throw new ArgumentException("Issuer cannot be null."),
                Audience = _jwtConfig.Audience ?? throw new ArgumentException("Audience cannot be null."),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // Create and return the token
            var tokenHandler = new JwtSecurityTokenHandler();
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
}
