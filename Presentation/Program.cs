using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;
using XMP.Application.Configration;
using XMP.Application.Extensions;
using XMP.Application.Interfaces;
using XMP.Application.Services;
using XMP.Domain.Repositories;
using XMP.Infrastructure.DbContext;
using XMP.Infrastructure.Repositories;


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<DapperDbContext>();

// Add services to the container.

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        // Configure Swagger to use JWT Authentication
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    }
    );


// Configure your JWT settings
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));

// Add JWT authentication

builder.Services.AddJwtAuthentication(builder.Configuration);

// Add AutoMapper (if you're using it)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add repository and service dependencies
builder.Services.AddAutoMapper(typeof(XMP.Application.Mappers.UserProfile));

// Add Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Use Swagger if in the development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Other middleware
app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
