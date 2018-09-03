using System;
using System.Collections.Concurrent;
using Hycon.Interfaces.Domain;
using Hycon.Interfaces.EventStore;

namespace Hycon.Infrastructure.Streams
{
    public class StreamLocator : IStreamLocator
    {
        private readonly ConcurrentDictionary<Guid, IStream> _streams = new ConcurrentDictionary<Guid, IStream>();

        public StreamLocator(IEventStore eventStore)
        {
            eventStore.BatchStreams.Subscribe(streams =>
            {
                foreach (var s in streams)
                    GetOrAdd(s);
            });
        }

        public Guid Key(IAggregate aggregate)
        {
            return aggregate.Id;
        }

        public IStream Find(Guid key)
        {
            _streams.TryGetValue(key, out var stream);
            return stream;
        }

        public IStream GetOrAdd(IAggregate aggregate)
        {
            var key = Key(aggregate);
            var stream = new Stream(key,aggregate.Version, aggregate.GetType());
            return _streams.GetOrAdd(key, stream);
        }

        public IStream GetOrAdd(IStream stream)
        {
            return _streams.GetOrAdd(stream.Key, stream);
        }
    }
}