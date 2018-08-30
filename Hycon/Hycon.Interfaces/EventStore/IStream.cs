using System;
using Hycon.Interfaces.Domain;

namespace Hycon.Interfaces.EventStore
{
    public interface IStream
    {
        /// <summary>
        /// Unique key identifying the stream
        /// </summary>
        Guid Key { get; }
        /// <summary>
        /// Last stream event version
        /// </summary>
        long Version { get; set; }
    }

    public interface IStreamLocator
    {
        // Key generation
        /// <summary>
        /// Generate key from aggregate instance
        /// </summary>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        Guid Key(IAggregate aggregate);

        /// <summary>
        /// Get the stream with the given id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IStream Find(Guid key);
        // Stream repository        
        /// <summary>
        /// Extract and cache the stream details for aggregate
        /// </summary>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        IStream GetOrAdd(IAggregate aggregate);
        /// <summary>
        /// Extract and cache the stream details for the target stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        IStream GetOrAdd(IStream stream);

    }
}