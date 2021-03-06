using System;
using System.Threading.Tasks;

namespace Hycon.Interfaces.Domain
{
    public interface IDomainRepository
    {
        /// <summary>
        /// Saves the events to event store and publish those to event bus
        /// </summary>
        /// <param name="es">The event sourced instance</param>
        Task Save<T>(T es) where T : class, IEventSourced;

        /// <summary>
        /// Rebuild from event history extracted from Event Store
        /// </summary>
        /// <param name="id">Event sourced guid</param>
        /// <typeparam name="T">Event sourced type</typeparam>
        /// <returns>Aggregate or null if no events found</returns>
        Task<T> Find<T>(Guid id) where T : class, IEventSourced,new();
    }
}