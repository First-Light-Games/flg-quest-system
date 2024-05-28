using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestSystem.Application.Common.Interfaces;
using QuestSystem.Infrastructure.MetricProviders.Playfab;
using QuestSystem.Infrastructure.Security;
using Refit;

namespace QuestSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Refit Playfab Settings
        services.AddRefitClient<IPlayfabAPI>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri($"https://{configuration.GetValue<string>("AppSettings:Playfab:PlayfabTitleId")}.playfabapi.com");
                c.DefaultRequestHeaders.Add("X-SecretKey", configuration.GetValue<string>("AppSettings:Playfab:PlayfabSecretKey"));
            });
        
        services.AddScoped<IMetricProvider, PlayfabMetricProvider>();
        services.AddScoped<ISecureDataService, SecureDataService>();
        
        return services;
    }
}


//Keep it here for a while... the first stage doesn't have any DB/Auth Implementation but we are aiming for that during Phase 2
// var connectionString = configuration.GetConnectionString("DefaultConnection");
//
// Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

// services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
// services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
//
// services.AddDbContext<ApplicationDbContext>((sp, options) =>
// {
//     options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
//
//     options.UseSqlServer(connectionString);
// });
//
// services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
//
// services.AddScoped<ApplicationDbContextInitialiser>();
//
// services.AddAuthentication()
//     .AddBearerToken(IdentityConstants.BearerScheme);
//
// services.AddAuthorizationBuilder();
//
// services.AddSingleton(TimeProvider.System);
