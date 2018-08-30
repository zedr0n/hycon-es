using System;
using Hycon.Interfaces;

namespace Hycon.Infrastructure
{
    public abstract class EventBase : IEvent
    {
        protected EventBase(Guid eventId, long timestamp)
        {
            EventId = eventId;
            Timestamp = timestamp;
        }

        public Guid EventId { get; }
        public long Timestamp { get; set; }
    }
}