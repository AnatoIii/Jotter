using JotterAPI.DAL;
using JotterAPI.DAL.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using XUnitJotterAPITests.Helpers;

namespace XUnitJotterAPITests
{
    public abstract class JotterTestDbContext : IDisposable
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;

        protected readonly JotterDbContext _dbContext;

        protected JotterTestDbContext()
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<JotterDbContext>()
                    .UseSqlite(_connection)
                    .Options;

            _dbContext = new JotterDbContext(options);
            _dbContext.Database.EnsureCreated();

            _dbContext.Seed();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
