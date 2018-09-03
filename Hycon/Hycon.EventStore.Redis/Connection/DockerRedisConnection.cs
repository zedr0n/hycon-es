using StackExchange.Redis;

namespace Hycon.EventStore.Redis
{
    public class DockerRedisConnection : IRedisConnection
    {
        private readonly ConnectionMultiplexer _connection;
        
        public DockerRedisConnection()
        {
            _connection = ConnectionMultiplexer.Connect("redis");
        }

        public IDatabase Database => _connection.GetDatabase();
        public ISubscriber GetSubscriber()
        {
            return _connection.GetSubscriber();
        }

    }
}