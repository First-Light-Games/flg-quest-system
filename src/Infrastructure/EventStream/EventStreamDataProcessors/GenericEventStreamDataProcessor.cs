using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using QuestSystem.Application.Common.Interfaces;
using QuestSystem.Application.Common.Interfaces.EventStream;
using QuestSystem.Application.Common.Models;

namespace QuestSystem.Infrastructure.EventStream.EventStreamProcessors;

/// <summary>
/// The EventStreamProcessor class is responsible for processing and enqueuing
/// incoming events into an event queue for further asynchronous processing.
/// This generic version is designed to handle event data in a type-agnostic way,
/// meaning it does not perform any transformations on the incoming data before
/// enqueuing it. The data type <typeparamref name="T"/> can represent any type of event,
/// whether it is a single object, an array, or complex nested structures.
///
/// In some scenarios, such as PlayFab webhook handling(Let's call it PlayStream), the incoming data might 
/// be dynamic in nature (e.g., a single JSON object or an array of objects). 
/// This processor, being generic, will directly enqueue the raw event data without
/// distinguishing between different formats. 
///
/// More specific processors could be implemented if you need to transform or
/// validate the incoming event before processing it.
///
/// Example:
/// - PlayFab might send a single event as a JSON object, or an array of events.
/// - This processor does not differentiate or transform this data; it simply
///   enqueues it as-is, relying on the consumer to handle the specifics.
///
/// </summary>
/// <typeparam name="T">The type of event data that will be processed.</typeparam>
public class GenericEventStreamDataProcessor<T> : IEventStreamDataProcessor<T> where T : EventStreamData
{
    private readonly ILogger<GenericEventStreamDataProcessor<T>> _logger;
    private readonly IEventStreamQueue<T> _eventStreamQueue;

    public GenericEventStreamDataProcessor(ILogger<GenericEventStreamDataProcessor<T>> logger, IEventStreamQueue<T> eventStreamQueue)
    {
        _logger = logger;
        _eventStreamQueue = eventStreamQueue;
    }

    
    /// <summary>
    /// Processes the incoming event by adding it to the event queue.
    /// Since this is a generic processor, the incoming event data is not transformed.
    /// It is directly enqueued in its raw form, allowing flexibility for different
    /// types of events (e.g., single objects, arrays, complex structures).
    /// 
    /// If you need to handle specific types of transformations (such as handling
    /// arrays vs. single objects), consider extending this class or creating a 
    /// specialized processor.
    /// </summary>
    /// <param name="eventData">The event data to process, which can be of any type <typeparamref name="T"/>.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ProcessEventDataAsync(T eventData)
    {
        
        
        _eventStreamQueue.EnqueueEvent(eventData);

        await Task.CompletedTask;
    }
}
