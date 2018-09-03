using System;
using System.Linq;
using System.Threading.Tasks;
using Hycon.Infrastructure.Exceptions;
using Hycon.Interfaces.Domain;
using Hycon.Interfaces.EventStore;
using NodaTime;

namespace Hycon.Infrastructure.Domain
{
    public class DomainRepository : IDomainRepository
    {
        private readonly IEventStore _eventStore;
        private readonly IStreamLocator _streams;
        private readonly IClock _clock;

        public DomainRepository(IEventStore eventStore, IStreamLocator streams, IClock clock)
        {
            _eventStore = eventStore;
            _streams = streams;
            _clock = clock;
        }

        public async Task Save<T>(T aggregate) where T : class, IAggregate
        {
            if (aggregate == null)
                return;

            var events = aggregate.GetUncommittedEvents().ToList();
            if (events.Count == 0)
                return;
            
            var expectedVersion = aggregate.Version - events.Count;
            
            var stream = _streams.GetOrAdd(aggregate);
            if ((await _eventStore.ReadStream(stream, expectedVersion)).Any())
                throw new ConcurrencyException(stream.Key);

            foreach (var @event in events)
            {
                @event.EventId = Guid.NewGuid(); 
                @event.Timestamp = _clock.GetCurrentInstant().ToUnixTimeMilliseconds();
            }
                   
            await _eventStore.WriteStream(stream, events);    
        }

        public async Task<T> Find<T>(Guid id) where T : class, IAggregate, new()
        {
            var stream = _streams.Find(id);
            if (stream == null)
                return null;

            var events = await _eventStore.ReadStream(stream, 0, int.MaxValue);
            var aggregate = new T();
            aggregate.LoadFrom<T>(id, events);
            
            return aggregate;
        }
    }
}