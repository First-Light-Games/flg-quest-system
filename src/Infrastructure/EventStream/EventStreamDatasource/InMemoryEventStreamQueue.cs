using System;
using System.Collections.Generic;
using QuestSystem.Application.Common.Interfaces;
using QuestSystem.Application.Common.Models;

namespace QuestSystem.Infrastructure.EventStream;

/// <summary>
/// InMemoryEventStreamQueue provides a simple in-memory queue for storing events
/// until they are consumed by a background service. This is a temporary
/// persistence mechanism, and can be replaced by more robust solutions like
/// a message queue or database.
/// </summary>
/// <typeparam name="T">The type of event data stored in the queue.</typeparam>
public class InMemoryEventStreamQueue<T> : IEventStreamQueue<T> where T: EventStreamData 
{
    private readonly Queue<T> _queue = new();
    
    public void EnqueueEvent(T eventData)
    {
        _queue.Enqueue(eventData);
    }

    public void EnqueueEvent(EventStreamData eventData)
    {
        throw new NotImplementedException();
    }

    public T DequeueEvent()
    {
        if (_queue.Count == 0)
        {
            throw new InvalidOperationException("Queue is Empty");
        }

        return _queue.Dequeue();
    }

    public bool HasEvents => _queue.Count > 0;
}
