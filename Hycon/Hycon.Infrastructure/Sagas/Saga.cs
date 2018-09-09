using System;
using System.Collections.Generic;
using Hycon.Infrastructure.Domain;
using Hycon.Interfaces;
using Hycon.Interfaces.Domain;
using Hycon.Interfaces.Sagas;

namespace Hycon.Infrastructure.Sagas
{
    public class Saga : EventSourced, ISaga
    {
        private readonly List<ICommand> _undispatchedCommands = new List<ICommand>();

        private void ClearUncommittedCommands()
        {
            lock(_undispatchedCommands) 
                _undispatchedCommands.Clear();
        }
        
        public ICommand[] GetUncommittedCommands()
        {
            lock (_undispatchedCommands)
                return _undispatchedCommands.ToArray();
        }

        public void SendCommand(ICommand command)
        {
            lock(_undispatchedCommands)
                _undispatchedCommands.Add(command);
        }

        public override void LoadFrom<T>(Guid id, IEnumerable<IEvent> pastEvents)
        {
            base.LoadFrom<T>(id, pastEvents);
            ClearUncommittedCommands();
        }
    }
}