using System.Threading.Tasks;
using Hycon.Interfaces.Domain;

namespace Hycon.Infrastructure.Bus
{
    public interface IBus
    {
        bool Command(ICommand command);
        Task CommandAsync(ICommand command);
    }
}