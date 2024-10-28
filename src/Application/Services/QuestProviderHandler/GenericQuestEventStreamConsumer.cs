using Microsoft.Extensions.Logging;
using QuestSystem.Application.Common.Interfaces.EventStream;
using QuestSystem.Application.Common.Models;

namespace QuestSystem.Application.Services.QuestProviderHandler;

public class GenericQuestEventStreamConsumer : IEventStreamConsumer<EventStreamData>
{

    private readonly ILogger<GenericQuestEventStreamConsumer> _logger;

    public GenericQuestEventStreamConsumer(ILogger<GenericQuestEventStreamConsumer> logger)
    {
        _logger = logger;
    }

    public void OnEventReceived(EventStreamData eventData)
    {
        _logger.LogInformation($"Class {this.GetType().Name} has received the event {eventData}");
    }

}
