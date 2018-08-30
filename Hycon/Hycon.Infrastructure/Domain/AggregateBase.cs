using System;
using System.Collections.Generic;
using Hycon.Interfaces;
using Hycon.Interfaces.Domain;

namespace Hycon.Infrastructure.Domain
{
    public abstract class AggregateBase : IAggregate
    {        
        public Guid Key { get; private set; }
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

        protected virtual void When(IEvent e)
        {
            lock (_changes)
            {
                Version++;
                _changes.Add(e);
            }
        }

        public T LoadFrom<T>(Guid id, IEnumerable<IEvent> pastEvents) where T : class,IAggregate
        {
            Key = id;
            foreach (var e in pastEvents)
                When(e);

            ClearUncommittedEvents();
            return this as T;
        }
    }
}