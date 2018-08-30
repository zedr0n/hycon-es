using System;
using System.Threading.Tasks;

namespace Hycon.Interfaces.Messaging
{
    public interface IMessageQueue
    {
        IObservable<IMessage> Messages { get; }

        Task PublishAsync(IMessage message);
    }
}