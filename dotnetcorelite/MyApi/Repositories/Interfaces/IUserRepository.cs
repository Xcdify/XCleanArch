using MyApi.Models;

namespace MyApi.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> CreateUserAsync(User user);
}
