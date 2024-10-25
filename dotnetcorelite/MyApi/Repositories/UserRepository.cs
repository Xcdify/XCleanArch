using AutoMapper;
using Dapper;
using MyApi.Models;
using MyApi.Repositories.Interfaces;
using MyApi.Data;

namespace MyApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DapperContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DapperContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        const string sql = "SELECT id, username, encrypted_password AS Password, email FROM users WHERE username = @Username";
        using var connection = _context.CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        return result;
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        const string sql = @"
            INSERT INTO users (username, encrypted_password, email)
            VALUES (@Username, @Password, @Email)";
        
        using var connection = _context.CreateConnection();
        var parameters = new
        {
            user.Username,
            user.Password,
            user.Email
        };
        var result = await connection.ExecuteAsync(sql, parameters);
        return result > 0;
    }
}
