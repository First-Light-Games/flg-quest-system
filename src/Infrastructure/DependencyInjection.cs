using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using QuestSystem.Application.Common.Interfaces;
using QuestSystem.Application.Common.Interfaces.EventStream;
using QuestSystem.Application.Common.Interfaces.Providers;
using QuestSystem.Application.Common.Models;
using QuestSystem.Application.Services.QuestProviderHandler;
using QuestSystem.Infrastructure.EventStream.EventStreamDataProcessors;
using QuestSystem.Infrastructure.MetricProviders.Playfab;
using QuestSystem.Infrastructure.Security;
using QuestSystem.Infrastructure.ServiceProviders.Playfab;
using QuestSystem.Infrastructure.ServiceProviders.Ygg;
using Refit;

namespace QuestSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        SetupRefitHttpClients(services, configuration);

        services.AddScoped<ISecureDataService, SecureDataService>();
        
        //Providers are usually ThirdParty services
        services.AddScoped<IMetricProvider, PlayfabMetricProvider>();
        services.AddScoped<IEventStreamDataProcessor<JsonElement>, PlayfabEventStreamDataProcessor>();
        services.AddKeyedScoped<IQuestProvider, YggQuestProvider>(typeof(YggQuestProviderHandler));
        
        services.AddLogging();
        
        return services;
    }

    private static void SetupRefitHttpClients(IServiceCollection services, IConfiguration configuration)
    {
        services.AddRefitClient<IPlayfabAPI>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri($"https://{configuration.GetValue<string>("AppSettings:Playfab:PlayfabTitleId")}.playfabapi.com");
                c.DefaultRequestHeaders.Add("X-SecretKey", configuration.GetValue<string>("AppSettings:Playfab:PlayfabSecretKey"));
            });
        
        services.AddRefitClient<IYggAPI>()
            .ConfigureHttpClient(c =>
            {
                var baseUrl = configuration.GetValue<string>("AppSettings:QuestProvider:Ygg:Endpoint");

                if (baseUrl.IsNullOrEmpty())
                {
                    throw new ArgumentException("Error setting up QuestProvider: YGG, Invalid/Null Base Endpoint");
                }

                c.BaseAddress = new Uri(baseUrl!);
                c.DefaultRequestHeaders.Add("API_KEY", configuration.GetValue<string>("AppSettings:QuestProvider:Ygg:ApiKey"));
            });
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
