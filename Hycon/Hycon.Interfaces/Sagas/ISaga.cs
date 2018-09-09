using Hycon.Interfaces.Domain;

namespace Hycon.Interfaces.Sagas
{
    public interface ISaga : IEventSourced
    {
        /// <summary>
        /// Commands not yet committed 
        /// </summary>
        ICommand[] GetUncommittedCommands();

        /// <summary>
        /// Add the commands to the dispatch queue
        /// </summary>
        void SendCommand(ICommand command);
    }
}