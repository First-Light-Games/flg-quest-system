using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HostInitActions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using QuestSystem.Application.Common.Interfaces;
using QuestSystem.Application.Common.Interfaces.EventStream;
using QuestSystem.Application.Common.Interfaces.Providers;
using QuestSystem.Application.Services.QuestProviderHandler;
using QuestSystem.Infrastructure.EventStream.EventStreamDataProcessors;
using QuestSystem.Infrastructure.MetricProviders.Playfab;
using QuestSystem.Infrastructure.Security;
using QuestSystem.Infrastructure.ServiceProviders.Playfab;
using QuestSystem.Infrastructure.ServiceProviders.RemoteConfig;
using QuestSystem.Infrastructure.ServiceProviders.Ygg;
using Refit;

namespace QuestSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        SetupRefitHttpClients(services, configuration);

        services.AddSingleton<IEventStreamDataProcessor<JsonElement>, PlayfabEventStreamDataProcessor>();
        
        //Providers are usually ThirdParty services
        //MetricsProviders are our main DataSource from players action/statistics inside the game (i.e. KillCount at the end of the Match)
        services.AddSingleton<IMetricProvider, PlayfabMetricProvider>();
        
        //QuestProviders are third parties QuestServices platforms, for example Ygg (Players can interact with their platform to get rewards they collected playing any game)
        services.AddSingleton<IQuestProvider<YggQuest>, YggQuestProvider>();

        services.AddAsyncServiceInitialization()
            .AddInitActionExecutor(serviceProvider => new ResolveQuestProviderExecutor(serviceProvider));
        
        services.AddScoped<ISecureDataService, SecureDataService>();
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

        services.AddRefitClient<IRemoteConfigAPI>()
            .ConfigureHttpClient(c =>
            {
                var baseUrl = configuration.GetValue<string>("AppSettings:RemoteConfig:Endpoint");

                if (baseUrl.IsNullOrEmpty())
                {
                    throw new ArgumentException("Error setting up QuestProvider: RemoteConfig, Invalid/Null Base Endpoint");
                }
                
                c.BaseAddress = new Uri(baseUrl!);
            });
    }
    
  
}

class ResolveQuestProviderExecutor(IServiceProvider serviceProvider) : IAsyncInitActionExecutor
{
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var questProviderTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass && !type.IsAbstract)
            .SelectMany(type => type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuestProvider<>))
                .Select(i => i))
            .Distinct()
            .ToList();

        foreach (var providerType in questProviderTypes)
        {
            var providerInstance = serviceProvider.GetService(providerType);
            
            if (providerInstance == null)
            {
                continue;
            }

            var initializeMethod = providerType.GetMethod("SetupActiveQuests");
            if (initializeMethod != null)
            {
                var task = (Task)initializeMethod.Invoke(providerInstance, null)!;
                task.Wait(); 
            }
        }

        return Task.CompletedTask;
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
