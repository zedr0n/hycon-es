using System;
using System.Collections.Concurrent;
using Hycon.Interfaces;
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

        public Guid Key(IEventSourced es)
        {
            return es.Id;
        }

        public IStream Find(Guid key)
        {
            _streams.TryGetValue(key, out var stream);
            return stream;
        }

        public IStream GetOrAdd(IEventSourced es)
        {
            var key = Key(es);
            var stream = new Stream(key,es.Version, es.GetType());
            return _streams.GetOrAdd(key, stream);
        }

        public IStream GetOrAdd(IStream stream)
        {
            return _streams.GetOrAdd(stream.Key, stream);
        }
    }
}