using System;

namespace Hycon.Interfaces
{
    public interface IMessage
    {
        /// <summary>
        /// Unique event id
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// Event Unix epoch timestamp
        /// </summary>
        long Timestamp { get; set; } 
    }
}