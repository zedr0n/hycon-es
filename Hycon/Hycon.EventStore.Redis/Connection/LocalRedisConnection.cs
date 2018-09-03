using StackExchange.Redis;

namespace Hycon.EventStore.Redis
{
    public class LocalRedisConnection : IRedisConnection
    {
        private readonly ConnectionMultiplexer _connection;
        
        public LocalRedisConnection()
        {
            _connection = ConnectionMultiplexer.Connect("localhost");
        }

        public IDatabase Database => _connection.GetDatabase();
        public ISubscriber GetSubscriber()
        {
            return _connection.GetSubscriber();
        }

    }
}