using ApplicationCore.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Web;

namespace FunctionalTests
{
    public class WebTestFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddEntityFrameworkInMemoryDatabase();

                var provider = services
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseInMemoryDatabase("FunctionalDB");
                    options.UseInternalServiceProvider(provider);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationContext>();

                    var loggerFactory = scopedServices.GetRequiredService<ILoggerFactory>();

                    var logger = scopedServices.GetRequiredService<ILogger<WebTestFixture>>();

                    var hostingEnviroment = scopedServices.GetRequiredService<IWebHostEnvironment>();

                    db.Database.EnsureCreated();

                    try
                    {
                        var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                        IdentityContextSeed.SeedAsync(userManager, roleManager).Wait();
                        ApplicationContextSeed.SeedAsync(db, userManager, Path.Combine(hostingEnviroment.WebRootPath, "img/example_image.jpg")).Wait();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                }
            });
        }
    }
}
