using System;
using System.Collections.Generic;
using Hycon.Interfaces;
using Hycon.Interfaces.Domain;

namespace Hycon.Infrastructure.Domain
{
    public abstract class AggregateRoot : IAggregate
    {        
        public Guid Id { get; protected set; }
        private readonly List<IEvent> _changes = new List<IEvent>();

        public long Version { get; private set; }

        public IEvent[] GetUncommittedEvents()
        {
            lock (_changes)
                return _changes.ToArray();
        }

        public void ClearUncommittedEvents()
        {
            lock(_changes)
                _changes.Clear();                
        }    

        protected void When(IEvent e)
        {
            lock (_changes)
            {
                ApplyEvent(e);
                Version++;
                _changes.Add(e);
            }
        }

        protected virtual void ApplyEvent(IEvent e) {}

        public T LoadFrom<T>(Guid id, IEnumerable<IEvent> pastEvents) where T : class,IAggregate
        {
            Id = id;
            foreach (var e in pastEvents)
                When(e);

            ClearUncommittedEvents();
            return this as T;
        }
    }
}