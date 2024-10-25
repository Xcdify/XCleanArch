using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApi.Configuration;
using MyApi.Repositories;
using MyApi.Repositories.Interfaces;
using MyApi.Services;
using MyApi.Services.Interfaces;
using MyApi.Data;
using Npgsql;
using System.Data;

namespace MyApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<DapperContext>();
        services.AddScoped<IDbConnection>(sp =>
            new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, AuthService>();

        services.Configure<JwtConfig>(configuration.GetSection("Jwt"));

        return services;
    }
}
