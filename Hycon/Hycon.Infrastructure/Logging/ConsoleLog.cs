using System;
using NodaTime;

namespace Hycon.Infrastructure.Logging
{
    public class ConsoleLog : ILog
    {
        private readonly IClock _clock;

        public ConsoleLog(IClock clock)
        {
            _clock = clock;
        }

        public Instant Now() => _clock.GetCurrentInstant();
        
        public void Write(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}