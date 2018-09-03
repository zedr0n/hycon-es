using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Hycon.Interfaces;
using Hycon.Interfaces.EventStore;

namespace Hycon.Infrastructure.Projections
{
    public class Projection
    {
        private readonly IEventStore _eventStore;
        private readonly ConcurrentDictionary<Guid, long> _streams = new ConcurrentDictionary<Guid, long>();

        public Projection(IEventStore eventStore)
        {
            _eventStore = eventStore;
            //Streams = _eventStore.BatchStreams;
        }

        protected virtual IObservable<IEnumerable<IStream>> Streams { get; set; }

        protected async Task Update(IEnumerable<IStream> streams)
        {
            var events = new List<IEvent>();
            foreach (var stream in streams)
            {
                var version = _streams.GetOrAdd(stream.Key,0);
                var streamEvents = (await _eventStore.ReadStream(stream, version)).ToList();
                
                events.AddRange(streamEvents); 
                _streams[stream.Key] = version + streamEvents.Count;    
            }

            foreach (var e in events.OrderBy(e => e.Timestamp))
                When(e);
        }

        protected virtual void When(IEvent e)
        {
                        
        }
        
        protected void Start(IObservable<IEnumerable<IStream>> streams)
        {
            streams.Subscribe(async s => await Update(s));
        }
    }
}