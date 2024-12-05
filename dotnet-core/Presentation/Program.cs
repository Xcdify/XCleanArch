using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using XMP.Application.Interfaces;
using XMP.Application.Services;
using XMP.Domain.Repositories;
using XMP.Infrastructure.Repositories;
using XMP.Infrastructure.DbContext;


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
builder.Services.AddSwaggerGen();

// Add AutoMapper (if you're using it)
builder.Services.AddAutoMapper(typeof(XMP.Application.Mappers.AxisBankTransactionProfile)); // Correct namespace for your AutoMapper profile

// Add repository and service dependencies
builder.Services.AddScoped<IAxisBankTransactionRepository, AxisBankTransactionRepository>();
builder.Services.AddScoped<IAxisBankTransactionService, AxisBankTransactionService>();

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
