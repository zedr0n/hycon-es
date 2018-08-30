using System;

namespace Hycon.Infrastructure.Exceptions
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(Guid id)
            : base($"A different version than expected was found in stream {id}")
        { }
    }

}