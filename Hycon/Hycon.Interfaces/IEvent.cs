using System;
using Hycon.Interfaces.Messaging;
using NodaTime;

namespace Hycon.Interfaces
{
    public interface IEvent : IMessage
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