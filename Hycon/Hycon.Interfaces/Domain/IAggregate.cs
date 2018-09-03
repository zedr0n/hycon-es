using System;
using System.Collections.Generic;
using System.Linq;

namespace Hycon.Interfaces.Domain
{
    public static class AggregateExtensions
    {
        public static string Name(this IAggregate aggregate)
        {
            var type = aggregate.GetType();
            return type.GenericTypeArguments.Aggregate(type.Name, (c, t) => c + t.Name);
        }
    }
    
    public interface IAggregate 
    {
        Guid Id { get; }
        
        /// <summary>
        /// Aggregate version ( for optimistic concurrency )
        /// </summary>
        long Version { get; }

        /// <summary>
        /// Events not yet committed to aggregate
        /// </summary>
        IEvent[] GetUncommittedEvents();
        void ClearUncommittedEvents();

        /// <summary>
        /// Hydrate the aggregate from event sequence
        /// </summary>
        /// <param name="id">Unique identifier for aggregate</param>
        /// <param name="pastEvents">Past event sequence</param>
        /// <typeparam name="T">Aggregate type</typeparam>
        /// <returns>Hydrated aggregate instance</returns>
        T LoadFrom<T>(Guid id,IEnumerable<IEvent> pastEvents) where T : class, IAggregate;
    }
}