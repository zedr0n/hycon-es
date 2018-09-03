using System.Threading.Tasks;
using Hycon.Interfaces;

namespace Hycon.Infrastructure.Pipes
{
    public interface IMessageQueue
    {
        Task PublishAsync(IEvent e);
    }
}