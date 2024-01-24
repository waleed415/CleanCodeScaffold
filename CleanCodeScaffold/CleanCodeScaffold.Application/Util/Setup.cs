using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanCodeScaffold.Application.Handlers.Interface;
using CleanCodeScaffold.Application.Handlers.Implimentation;
using CleanCodeScaffold.Infrastructure.Util;
using FluentValidation;
using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Application.Validators;

namespace CleanCodeScaffold.Application.Util
{
    public static class Setup
    {
        public static void SetupApplication(this IServiceCollection services, string connectionString)
        {
            services.SetupInfrastructure(connectionString);
            services.SetupValidators();
            services.SetupHandlers();
        }
        private static void SetupValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<WeatherVM>, WeatherValidator>();
        }
        private static void SetupHandlers(this IServiceCollection services)
        {
            services.AddScoped<IWeatherHandler, WeatherHandler>();
        }
    }
}
