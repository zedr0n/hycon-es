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
    
    public interface IAggregate : IEventSourced
    {

    }
}