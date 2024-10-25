using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace MyApi.Data;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    public IDbConnection CreateConnection()
        => new NpgsqlConnection(_connectionString);
}
