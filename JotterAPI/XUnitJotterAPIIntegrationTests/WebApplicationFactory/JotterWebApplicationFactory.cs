using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using JotterAPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using XUnitJotterAPIIntegrationTests.Helpers;

namespace XUnitJotterAPIIntegrationTests.WebApplicationFactory
{
    public class JotterWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public static bool _dbSeeded = false;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<JotterDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<JotterDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DataSource=:memory:");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<JotterDbContext>();

                    db.Database.EnsureCreated();

                    if (!_dbSeeded)
                    {
                        _dbSeeded = true;
                        DbContextSeeding.Seed(db);
                    }
                }
            });
        }
    }
}
