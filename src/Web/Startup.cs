using ApplicationCore.Constants;
using ApplicationCore.Entities.Identity;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.IO;
using Web.Configuration;
using Web.Models;

namespace Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Email>(Configuration.GetSection("Email"));
            services.Configure<FileSettings>(Configuration.GetSection("FileSettings"));

            // Core services
            services.AddCoreServices();

            // Web services
            services.AddWebServices();

            // Database
            if (!_environment.IsEnvironment("Testing"))
            {
                // Use DockerConnection for Docker
                services.AddDbContext<ApplicationContext>(options =>
                            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationContext>();
            }

            // Identity
            services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordPolicy>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;
            });

            // Data protection
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Keys"))
                                        .SetDefaultKeyLifetime(TimeSpan.FromDays(180));

            // Cookie
            services.ConfigureCookieSettings();

            // Localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };

                var provider = new CookieRequestCultureProvider()
                {
                    CookieName = "culture"
                };

                options.RequestCultureProviders.Insert(0, provider);

                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            // Other services
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = CookieNamesConstants.Antiforgery;
            });
            services.AddHttpClient();
            services.AddControllersWithViews().AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(SharedResource));
            }).AddViewLocalization();
            services.AddMemoryCache();
            services.AddRazorPages();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePagesWithReExecute("/HttpError", "?code={0}");

            app.UseRequestLocalization();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{certificateId?}");
                endpoints.MapControllerRoute(
                    name: "public",
                    pattern: "Public/{uniqueUrl}",
                    defaults: new { controller = "Public", action = "Index" });
                endpoints.MapRazorPages();
            });
        }
    }
}
