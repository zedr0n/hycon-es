using System;
using Hycon.Interfaces;

namespace Hycon.Infrastructure
{
    public abstract class EventBase : IEvent
    {
        public Guid Id { get; set; }
        public long Timestamp { get; set; }
    }
}