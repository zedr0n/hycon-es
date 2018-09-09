using Hycon.Interfaces;
using Hycon.Interfaces.Domain;

namespace Hycon.Infrastructure.Domain
{
    public class Command : ICommand
    {
        public long Timestamp { get; set; }
    }
}