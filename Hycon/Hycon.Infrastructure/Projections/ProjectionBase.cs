using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Hycon.Interfaces;
using Hycon.Interfaces.EventStore;

namespace Hycon.Infrastructure.Projections
{
    public class ProjectionBase
    {
        private readonly IEventStore _eventStore;
        private readonly ConcurrentDictionary<Guid, long> _streams = new ConcurrentDictionary<Guid, long>();

        public ProjectionBase(IEventStore eventStore)
        {
            _eventStore = eventStore;
            Streams = _eventStore.Streams;
        }

        protected virtual IObservable<IStream> Streams { get; }

        private async Task Update(IStream s)
        {
            var version = _streams.GetOrAdd(s.Key, 0);
            var events = await _eventStore.ReadStream(s, version);
            foreach (var e in events)
            {
                When(s,e);
                version++;
            }

            _streams[s.Key] = version;
        }

        protected virtual void When(IStream s, IEvent e)
        {
                        
        }
        
        public void Start()
        {
            Streams.Subscribe(async s => await Update(s));
        }
    }
}