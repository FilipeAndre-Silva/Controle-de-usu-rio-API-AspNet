using Microsoft.Extensions.DependencyInjection;
using UserControl.Infra.Context;
using UserControl.Infra.Repository;

namespace UserControl.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection SolvesDependencies(this IServiceCollection services)
        {
            services.AddScoped<UserDbContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}