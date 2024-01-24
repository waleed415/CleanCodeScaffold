using CleanCodeScaffold.Domain.Interfaces;
using CleanCodeScaffold.Infrastructure.AuditServices;
using CleanCodeScaffold.Infrastructure.Persistence;
using CleanCodeScaffold.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Infrastructure.Util
{
    public static class Setup
    {
        public static void SetupInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("CleanCodeScaffold.Infrastructure"))
            );
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            RepoInejctor(services);
        }

        private static void RepoInejctor(IServiceCollection services)
        {
            services.AddScoped<IWeatherRepository, WeatherRepository>();
        }

    }
}
