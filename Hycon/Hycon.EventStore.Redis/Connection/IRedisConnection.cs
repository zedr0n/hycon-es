using StackExchange.Redis;

namespace Hycon.EventStore.Redis
{
    public interface IRedisConnection
    {
        IDatabase Database { get; }
        ISubscriber GetSubscriber();
    }
}