using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using ApplicationCore.Constants;

namespace Web.Configuration
{
    public static class CookieSettings
    {
        public static IServiceCollection ConfigureCookieSettings(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
                options.Cookie.Name = CookieNamesConstants.Identity;
                options.ExpireTimeSpan = TimeSpan.FromDays(21);
                options.LoginPath = "/Identity/Account/Login";
                options.Cookie.HttpOnly = true;
            });

            return services;
        }
    }
}
