using System.Threading.Tasks;
using Hycon.Interfaces;
using Hycon.Interfaces.Domain;

namespace Hycon.Infrastructure.Pipes
{
    public interface IBus
    {
        bool Command(ICommand command);
        Task CommandAsync(ICommand command);

        TResult Query<TResult>(IQuery<TResult> query);
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}