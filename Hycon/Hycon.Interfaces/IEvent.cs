using System;

namespace Hycon.Interfaces
{
    public interface IEvent 
    {
        /// <summary>
        /// Unique event id
        /// </summary>
        Guid EventId { get; set; }
        /// <summary>
        /// Event Unix epoch timestamp
        /// </summary>
        long Timestamp { get; set; }
    }
}