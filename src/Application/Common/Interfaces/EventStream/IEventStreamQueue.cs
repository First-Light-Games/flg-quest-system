using QuestSystem.Application.Common.Models;

namespace QuestSystem.Application.Common.Interfaces;

/// <summary>
/// The IEventStreamQueue<T> interface provides a mechanism for storing and retrieving events.
/// It defines methods to enqueue and dequeue events, and a property to check if events are pending.
/// </summary>
/// <typeparam name="T">Represents the type of event data the queue will handle.</typeparam>
public interface IEventStreamQueue<T> where T : EventStreamData
{
    
    /// <summary>
    /// Adds a new event to the queue.
    /// </summary>
    /// <param name="eventData">The event data of type T to be enqueued.</param>
    void EnqueueEvent(T eventData);

    /// <summary>
    /// Removes and returns the next event from the queue.
    /// </summary>
    /// <returns>The next event in the queue.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no events are available to dequeue.</exception>
    T DequeueEvent();

    /// <summary>
    /// Indicates whether the queue contains any events.
    /// </summary>
    /// <value>Returns true if there are events in the queue; otherwise, false.</value>
    bool HasEvents { get; }
}
