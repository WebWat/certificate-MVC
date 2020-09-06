using Microsoft.Extensions.DependencyInjection;
using Web.Interfaces;
using Web.Services;

namespace Web.Configuration
{
    public class ConfigureWebServices
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<ICertificateViewModelService, CertificateViewModelService>();
            services.AddScoped<ILinkViewModelService, LinkViewModelService>();
            services.AddScoped<IPublicViewModelService, PublicViewModelService>();
            services.AddScoped<IRatingViewModelService, RatingViewModelService>();
            services.AddScoped<IAdminViewModelService, AdminViewModelService>();
            services.AddScoped<IModeratorViewModelService, ModeratorViewModelService>();
            services.AddScoped<IEventViewModelService, EventViewModelService>();
        }
    }
}
