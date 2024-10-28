using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using QuestSystem.Application.Common.Interfaces.EventStream;
using QuestSystem.Application.Common.Models;

namespace QuestSystem.Infrastructure.EventStream.EventStreamDataProcessors;

public class PlayfabEventStreamDataProcessor : IEventStreamDataProcessor<JsonElement>
{
    private readonly ILogger<PlayfabEventStreamDataProcessor> _logger;
    private readonly IEnumerable<IEventStreamConsumer<EventStreamData>> _streamConsumers;

    public PlayfabEventStreamDataProcessor(ILogger<PlayfabEventStreamDataProcessor> logger, IEnumerable<IEventStreamConsumer<EventStreamData>> streamConsumers)
    {
        _logger = logger;
        _streamConsumers = streamConsumers;
    }

    
    public Task ProcessEventDataAsync(JsonElement eventData)
    {
        if (eventData.ValueKind != JsonValueKind.Array)
        {
            if (eventData.ValueKind == JsonValueKind.Object)
            {
                ProcessEventStreamDataAndPublish(eventData);
            }
        }
        else
        {
            foreach (var ev in eventData.EnumerateArray())
            {
                ProcessEventStreamDataAndPublish(eventData);
            }
        }

        return Task.CompletedTask;
    }

    private void ProcessEventStreamDataAndPublish(JsonElement rawData)
    {
        var entityId = rawData.GetString("EntityId");
        var eventName = rawData.GetString("EventName"); 
        var statisticName = rawData.GetString("StatisticName");
        var statisticPreviousValue = rawData.GetString("StatisticPreviousValue");
        var statisticValue = rawData.GetString("StatisticValue");
        
        if (!string.IsNullOrEmpty(entityId) &&
            !string.IsNullOrEmpty(eventName) &&
            !string.IsNullOrEmpty(statisticName) &&
            int.TryParse(statisticPreviousValue, out int statisticPreviousValueInt) &&
            int.TryParse(statisticValue, out int statisticValueInt))
        {
            var playfabEventData = new EventStreamData(
                entityId,
                eventName,
                statisticName,
                statisticPreviousValueInt,
                statisticValueInt
            );

            foreach (var registeredConsumer in _streamConsumers)
            {
                registeredConsumer.OnEventReceived(playfabEventData);
            }
        }
        else
        {
            _logger.LogError($"Couldn't process/parse Playstream event with name {eventName}");
        }
    }
}
