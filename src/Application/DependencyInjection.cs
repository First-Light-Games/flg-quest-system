using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using QuestSystem.Application.Common.Behaviours;
using QuestSystem.Application.Common.Interfaces.EventStream;
using QuestSystem.Application.Common.Models;
using QuestSystem.Application.Services.QuestProviderHandler;
using QuestSystem.Domain.Services;

namespace QuestSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<QuestService>();
        services.AddSingleton<IEventStreamConsumer<EventStreamData>, YggQuestProviderHandler>();
        
        
        //Register BackgroundServices
        // services.AddHostedService<EventStreamBroadcastService>();
        
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });

        return services;
    }
}
