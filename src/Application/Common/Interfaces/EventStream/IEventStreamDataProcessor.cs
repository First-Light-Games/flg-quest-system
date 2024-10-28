using System.Threading.Tasks;
using QuestSystem.Application.Common.Models;

namespace QuestSystem.Application.Common.Interfaces.EventStream;

/// <summary>
/// Interface for the EventStreamDataProcessor, which handles/validates incoming events data before enqueuing/processing it.
/// </summary>
/// <typeparam name="T">The type of event data that will be processed.</typeparam>
public interface IEventStreamDataProcessor<T>
{
    /// <summary>
    /// Processes the incoming event by enqueuing it into the event queue.
    /// </summary>
    /// <param name="eventData">The event data to process.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ProcessEventDataAsync(T eventData);
}
