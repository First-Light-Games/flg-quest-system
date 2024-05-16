using System.Collections.Generic;

namespace QuestSystem.Domain.Common
{
    public abstract class BaseModel
    {
        // This can easily be modified to be BaseEntity<T> and public T Id to support different key types.
        // Using non-generic integer types for simplicity
        // For the initial purpose of FLG-QuestSystem we are not going to use persistence-layer so we can let this one 
        // public int Id { get; set; }

        private readonly List<BaseEvent> _domainEvents = new List<BaseEvent>();

        public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
