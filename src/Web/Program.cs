using ApplicationCore.Constants;
using ApplicationCore.Entities.Identity;
using ApplicationCore.Models;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using Web;
using Web.Configuration;
using Web.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Email>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<FileSettings>(builder.Configuration.GetSection("FileSettings"));

// Core services.
builder.Services.AddCoreServices();

// Web services.
builder.Services.AddWebServices();

// Identity.
builder.Services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordPolicy>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.AllowedForNewUsers = true;
});

// Data protection.
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Keys"))
                            .SetDefaultKeyLifetime(TimeSpan.FromDays(180));

// Cookie.
builder.Services.ConfigureCookieSettings();

// Localization.
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
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

// Other services.
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = CookieNamesConstants.Antiforgery;
});

builder.Services.AddControllersWithViews().AddDataAnnotationsLocalization(options =>
{
    options.DataAnnotationLocalizerProvider = (type, factory) =>
        factory.Create(typeof(SharedResource));
}).AddViewLocalization();

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

// Database.
if (!builder.Environment.IsEnvironment("Testing"))
{
    // Use DockerConnection for Docker
    builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationContext>();
}

// ServiceProvider.
var serviceProvider = builder.Services.BuildServiceProvider();

// Seed database.
using (var scope = serviceProvider.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try
    {
        var applicationContext = services.GetRequiredService<ApplicationContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await IdentityContextSeed.SeedAsync(userManager, roleManager);
        await ApplicationContextSeed.SeedAsync(applicationContext, userManager, "/img/example_image.jpg");
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Build and run app.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseBrowserLink();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStatusCodePagesWithReExecute("/HttpError", "?code={0}");

app.UseRequestLocalization();
app.UseStaticFiles();
app.UseCookiePolicy();

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

app.Run();
