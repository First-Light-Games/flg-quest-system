using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuestSystem.Application.Common.Interfaces;
using QuestSystem.Application.Common.Interfaces.EventStream;
using QuestSystem.Application.Common.Models;

namespace QuestSystem.Application.Services.EventStream;

public class EventStreamBroadcastService : BackgroundService
{

    private readonly ILogger<EventStreamBroadcastService> _logger;
    private readonly IEventStreamQueue<EventStreamData> _eventStreamQueue;
    private readonly IEnumerable<IEventStreamConsumer<EventStreamData>> _eventStreamConsumers;
    

    public EventStreamBroadcastService(ILogger<EventStreamBroadcastService> logger, IEventStreamQueue<EventStreamData> eventStreamQueue, IEnumerable<IEventStreamConsumer<EventStreamData>> eventStreamConsumers)
    {
        _logger = logger;
        _eventStreamQueue = eventStreamQueue;
        _eventStreamConsumers = eventStreamConsumers;
    }

    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("EventStreamBroadcastService started");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                while (_eventStreamQueue.HasEvents)
                {
                    var eventData = _eventStreamQueue.DequeueEvent();
                    BroadcastEvent(eventData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing event.");
            }
            
            await Task.Delay(100, cancellationToken);
        }
        
        _logger.LogInformation("CancellationToken signal received. stopping EventStreamBroadcastService!");
    }

    
    private void BroadcastEvent(EventStreamData eventData)
    {
        foreach (var consumer in _eventStreamConsumers)
        {
            try
            {
                consumer.OnEventReceived(eventData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while broadcasting to {consumer.GetType().Name}");
            }
        }
    }
}
