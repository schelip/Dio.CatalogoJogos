using System.Linq;
using Dio.CatalogoJogos.Api;
using Dio.CatalogoJogos.Api.Data.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dio.CatalogoJogos.Test
{
    public class TestingWebAppFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CatalogoJogosDbContext>));

                if (dbContext != null)
                    services.Remove(dbContext);

                services.AddDbContext<CatalogoJogosDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestingDb");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<CatalogoJogosDbContext>();

                    db.Database.EnsureCreated();
                }
            });
        }
    }
}
