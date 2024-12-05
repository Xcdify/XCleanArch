using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;


namespace XMP.Infrastructure.DbContext
{
    public class DapperDbContext : IDisposable
    {
        private readonly IDbConnection _connection;

        public DapperDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("dbPostgreSQLConnection");
            _connection = new NpgsqlConnection(connectionString);
        }

        public IDbConnection GetConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection;
        }

        public void Dispose()
        {
            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
            _connection.Dispose();
        }
    }
}