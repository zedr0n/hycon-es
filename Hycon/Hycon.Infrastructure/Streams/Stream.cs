using System;
using Hycon.Interfaces.EventStore;

namespace Hycon.Infrastructure.Streams
{
    public class Stream : IStream
    {
        public Stream(Guid key, long version, Type type)
        {
            Key = key;
            Version = version;
            EventSourcedType = type;
        }

        public Guid Key { get; }
        public long Version { get; set; }
        public Type EventSourcedType { get; set; }
    }
}