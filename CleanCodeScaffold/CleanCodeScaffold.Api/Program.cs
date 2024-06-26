using CleanCodeScaffold.Api.Util;
using CleanCodeScaffold.Application.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
#if (enableLogging)
builder.Host.UseSerilog((context, logConfigs) =>
{
    logConfigs.ReadFrom.Configuration(new ConfigurationBuilder()
    .AddJsonFile("seri-log.json")
    .Build());
});
#endif
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CleanCodeScaffold API", Version = "v1" });
    c.OperationFilter<FiltersSwaggerConfigs>();
    // Add JWT Authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // Must be lower case
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
});

builder.Services.SetupApplication(builder.Configuration.GetConnectionString("default"), builder.Configuration);
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


await app.ApplyPendingMigrations();
#if (enableLogging)
app.UseSerilogRequestLogging();
#endif
app.UseMiddleware<LocalizationMiddleware>();
app.MapControllers();

app.Run();
