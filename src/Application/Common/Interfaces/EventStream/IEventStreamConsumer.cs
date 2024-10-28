using QuestSystem.Application.Common.Models;

namespace QuestSystem.Application.Common.Interfaces.EventStream;

/// <summary>
/// Interface for the EventStreamProcessor, which handles the processing of incoming events.
/// </summary>
public interface IEventStreamConsumer<T> where T : EventStreamData
{
    /// <summary>
    /// Processes the incoming event by enqueuing it into the event queue.
    /// </summary>
    /// <param name="eventData">The event data to process.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    void OnEventReceived(EventStreamData eventStreamData);
    
}
