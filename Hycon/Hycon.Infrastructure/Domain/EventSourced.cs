using System;
using System.Collections.Generic;
using Hycon.Interfaces;
using Hycon.Interfaces.Domain;

namespace Hycon.Infrastructure.Domain
{
    public abstract class EventSourced : IEventSourced 
    {        
        public Guid Id { get; protected set; }
        private readonly List<IEvent> _changes = new List<IEvent>();
        private readonly Dictionary<Type, Action<IEvent>> _handlers = new Dictionary<Type, Action<IEvent>>();

        public long Version { get; private set; }

        protected void Register<TEvent>(Action<TEvent> handler) where TEvent : class, IEvent
        {
            _handlers.Add(typeof(TEvent), e => handler(e as TEvent));
        }
        
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

        protected virtual void ApplyEvent(IEvent e)
        {
            if (e == null)
                return;
            
            if (_handlers.TryGetValue(e.GetType(), out var handler))
                handler(e);
        }
        
        public virtual void LoadFrom<T>(Guid id, IEnumerable<IEvent> pastEvents) where T : class, IEventSourced
        {
            Id = id;
            foreach (var e in pastEvents)
                When(e);

            ClearUncommittedEvents();
        }
    }
}