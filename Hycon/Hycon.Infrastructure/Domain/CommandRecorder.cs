using System;
using System.Threading.Tasks;
using Hycon.Infrastructure.Exceptions;
using Hycon.Infrastructure.Logging;
using Hycon.Interfaces;
using Hycon.Interfaces.Domain;
using Hycon.Interfaces.EventStore;
using NodaTime;

namespace Hycon.Infrastructure.Domain
{
    public class CommandRecorder<T> : ICommandHandler<T> where T : ICommand
    {
        private readonly ICommandHandler<T> _handler;
        private readonly IEventStore _eventStore;
        private readonly ILog _log;
        private readonly IClock _clock;
        
        public CommandRecorder(ICommandHandler<T> handler, IEventStore eventStore, ILog log, IClock clock)
        {
            _handler = handler;
            _eventStore = eventStore;
            _log = log;
            _clock = clock;
        }

        public async Task Handle(T command)
        {
            _log.WriteLine("Entering handler of " + command.GetType().Name);
            try
            {
                await _handler.Handle(command);
            }
            catch (Exception e)
            {
                _log.WriteLine(e.Message);
                
                // retry the command in case of concurrency exception
                if(e is ConcurrencyException)
                    await _handler.Handle(command);
                throw;
            }

            command.Timestamp = _clock.GetCurrentInstant().ToUnixTimeMilliseconds();
            await _eventStore.AppendCommand(command);
        }
    }
}