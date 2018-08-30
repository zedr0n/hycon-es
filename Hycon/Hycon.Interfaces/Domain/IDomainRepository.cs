using System;
using System.Threading.Tasks;

namespace Hycon.Interfaces.Domain
{
    public interface IDomainRepository
    {
        /// <summary>
        /// Saves the aggregate events to event store and publish those to event bus
        /// </summary>
        /// <param name="aggregate">The aggregate instance</param>
        Task Save<T>(T aggregate) where T : class, IAggregate;

        /// <summary>
        /// Rebuild the aggregate from event history extracted from Event Store
        /// </summary>
        /// <param name="id">The aggregate guid</param>
        /// <typeparam name="T">Aggregate type</typeparam>
        /// <returns>Aggregate or null if no events found</returns>
        Task<T> Find<T>(Guid id) where T : class, IAggregate,new();
    }
}