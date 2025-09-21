using Minimal.Api.Domain.Interfaces;
using Minimal.Api.Domain.Service;

namespace Minimal.Api.Infra.Ioc
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
