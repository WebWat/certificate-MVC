using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Infrastructure.Identity;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Configuration
{
    public class ConfigureCoreServices
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EFCoreRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<IUrlGenerator, UrlGenerator>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}
