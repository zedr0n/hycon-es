using System.Threading.Tasks;
using Hycon.Infrastructure.Pipes;
using Hycon.Interfaces;

namespace Hycon.CrossCuttingConcerns
{
    public class MessageQueue : IMessageQueue
    {
        public async Task PublishAsync(IEvent e)
        {
            
        }
    }
}