using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuestSystem.Application.Common.Interfaces.EventStream;
using QuestSystem.Application.Common.Interfaces.Providers;
using QuestSystem.Application.Common.Models;

namespace QuestSystem.Application.Services.QuestProviderHandler;

public class YggQuestProviderHandler : IEventStreamConsumer<EventStreamData>
{

    private readonly ILogger<YggQuestProviderHandler> _logger;
    private readonly IQuestProvider _questProvider;

    public YggQuestProviderHandler(ILogger<YggQuestProviderHandler> logger, [FromKeyedServices(typeof(YggQuestProviderHandler))]IQuestProvider questProvider)
    {
        _logger = logger;
        _questProvider = questProvider;
    }

    public void OnEventReceived(EventStreamData eventData)
    {
        // _questProvider.Sub
    }

}
