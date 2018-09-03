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
            AggregateType = type;
        }

        public Guid Key { get; }
        public long Version { get; set; }
        public Type AggregateType { get; set; }
    }
}