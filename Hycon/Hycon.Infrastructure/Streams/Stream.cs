using System;
using Hycon.Interfaces.EventStore;

namespace Hycon.Infrastructure.Streams
{
    public class Stream : IStream
    {
        public Stream(Guid key, long version)
        {
            Key = key;
            Version = version;
        }

        public Guid Key { get; }
        public long Version { get; set; }
    }
}