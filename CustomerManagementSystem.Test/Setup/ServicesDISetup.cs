using CustomerManagementSystem.Data;
using CustomerManagementSystem.Interfaces;
using CustomerManagementSystem.Test.Mocks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Test.Setup
{
    public class ServicesDISetup : IDisposable
    {
        public ServiceCollection services { get; private set; }
        public ServiceProvider ServiceProvider { get; protected set; }
        public ApplicationDBContext DbContext { get; set; }

        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;

        public ServicesDISetup()
        {
            services = new ServiceCollection();

            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();

            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlite(_connection));
            services.AddScoped<IBankService, MockBanks>();
            ServiceProvider = services.BuildServiceProvider();
            DbContext = ServiceProvider.GetService<ApplicationDBContext>();
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            //dispose DBContext here
            _connection.Close();

            DbContext.Dispose();
        }
    }
}
