using Microsoft.Extensions.DependencyInjection;
using CleanCodeScaffold.Application.Handlers.Interface;
using CleanCodeScaffold.Application.Handlers.Implimentation;
using CleanCodeScaffold.Infrastructure.Util;
using FluentValidation;
using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Application.Validators;
using CleanCodeScaffold.Domain.Entities;
using CleanCodeScaffold.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using CleanCodeScaffold.Application.Dtos.Configs;

namespace CleanCodeScaffold.Application.Util
{
    public static class Setup
    {
        public static void SetupApplication(this IServiceCollection services, string connectionString, IConfiguration configuration)
        {
            services.SetupInfrastructure(connectionString);
            services.SetupValidators();
            services.SetupHandlers();
            services.SetupIdentity();
            services.SetupConfigs(configuration);
        }
        private static void SetupValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<WeatherVM>, WeatherValidator>();
            services.AddScoped<IValidator<LoginVM>, LoginValidator>();
            services.AddScoped<IValidator<RegisterVM>, RegisterValidator>();
        }
        private static void SetupHandlers(this IServiceCollection services)
        {
            services.AddScoped<IWeatherHandler, WeatherHandler>();
            services.AddScoped<IUserHandler, UserHandler>();
        }

        private static void SetupIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<User>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<AppDBContext>();
        }

        private static void SetupConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTConfigs>(configuration.GetSection("JWTConfigs"));
        }
    }
}
