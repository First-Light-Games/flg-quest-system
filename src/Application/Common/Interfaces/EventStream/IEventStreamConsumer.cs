using QuestSystem.Application.Common.Models;

namespace QuestSystem.Application.Common.Interfaces.EventStream;

/// <summary>
/// Interface for the classes that will act as Consumers from the EventStream, 
/// </summary>
public interface IEventStreamConsumer<T> where T : EventStreamData
{
    /// <summary>
    /// Processes the incoming event.
    /// </summary>
    /// <param name="eventData">The event data to process.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    void OnEventReceived(EventStreamData eventStreamData);

    /// <summary>
    /// Configures the consumer to set up any initial state, subscriptions, filter or resources needed 
    /// for handling events from the EventStream.
    /// </summary>
    void ConfigureEventStreamConsumer();
}
