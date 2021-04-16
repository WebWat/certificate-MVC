using Microsoft.Extensions.DependencyInjection;
using Web.Interfaces;
using Web.Services;

namespace Web.Configuration
{
    public static class ConfigureWebServices
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddScoped<IStageService, StageService>();
            services.AddScoped<ICertificateViewModelService, CertificateViewModelService>();
            services.AddScoped<ILinkViewModelService, LinkViewModelService>();
            services.AddScoped<IPublicViewModelService, PublicViewModelService>();
            services.AddScoped<IAdminViewModelService, AdminViewModelService>();
            services.AddScoped<IModeratorViewModelService, ModeratorViewModelService>();
            services.AddScoped<IEventViewModelService, EventViewModelService>();
            services.AddScoped<ICachedPublicViewModelService, CachedPublicViewModelService>();
            services.AddScoped<IEmailTemplate, EmailTemplate>();

            return services;
        }
    }
}
