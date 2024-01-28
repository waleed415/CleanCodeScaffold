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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;


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
            services.SetupTokenValidation(configuration);
        }
        private static void SetupValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<WeatherVM>, WeatherValidator>();
            services.AddScoped<IValidator<LoginVM>, LoginValidator>();
            services.AddScoped<IValidator<RegisterVM>, RegisterValidator>();
            services.AddScoped<IValidator<ChangePasswordVM>, ChangePasswordValidator>();
            services.AddScoped<IValidator<ResetPasswordVM>, ResetPasswordValidator>();
        }
        private static void SetupHandlers(this IServiceCollection services)
        {
            services.AddScoped<IWeatherHandler, WeatherHandler>();
            services.AddScoped<IUserHandler, UserHandler>();
        }

        private static void SetupIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<User>(options =>
            {
                options.Tokens.PasswordResetTokenProvider = typeof(PasswordResetTokenProvider<User>).Name.Split("`")[0];
            })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<PasswordResetTokenProvider<User>>("PasswordResetTokenProvider"); 
        }

        private static void SetupConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTConfigs>(configuration.GetSection("JWTConfigs"));
        }

        private static void SetupTokenValidation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration.GetValue<string>("JWTConfigs:Issuer"),
                ValidAudience = configuration.GetValue<string>("JWTConfigs:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWTConfigs:Key"))),
                ClockSkew = TimeSpan.Zero
            };
        });


        }
    }
}
