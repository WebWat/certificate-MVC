using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using Web.Configuration;
using ApplicationCore.Constants;
using Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using System.Linq;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Email>(Configuration.GetSection("Email"));
            services.Configure<FileSettings>(Configuration.GetSection("FileSettings"));

            //Core services
            services.AddCoreServices();

            //Web services
            services.AddWebServices();

            //Database
            services.AddDbContext<ApplicationContext>(options =>
                            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); //Use DockerConnection for Docker

            //Identity
            services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordPolicy>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;
            });

            //Cookie
            services.ConfigureCookieSettings();

            //Localization
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

            //Other services
            services.AddScoped<IUrlShortener, UrlShortener>();
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = CookieNamesConstants.Antiforgery;
            });
            services.AddHttpClient();
            services.AddControllersWithViews().AddDataAnnotationsLocalization(options => {
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
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "public",
                    pattern: "Public/{uniqueUrl}",
                    defaults: new { controller = "Public", action = "Index" });
                endpoints.MapRazorPages();
            });
        }
    }
}
