using MyApi.Models;

namespace MyApi.Services.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(LoginRequest request);
    Task<bool> RegisterAsync(RegisterRequest request);
}
