using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Hycon.EventStore.Redis.Extensions;
using Hycon.Infrastructure.Domain;
using Hycon.Infrastructure.Exceptions;
using Hycon.Infrastructure.Streams;
using Hycon.Interfaces;
using Hycon.Interfaces.Domain;
using Hycon.Interfaces.EventStore;
using Hycon.Interfaces.Messaging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Hycon.EventStore.Redis
{
    public class RedisEventStore : IEventStore
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        private readonly IMessageQueue _messageQueue;
        private readonly IRedisConnection _redis;
        private readonly RedisKey _streamsKey = "EventStore::streams";

        public RedisEventStore(IMessageQueue messageQueue, IRedisConnection redis)
        {
            _messageQueue = messageQueue;
            _redis = redis;
            
            Streams = Observable.Create(async (IObserver<IStream> observer) =>
            {
                var values = await Db.HashValuesAsync(_streamsKey); 
                foreach (var value in values)
                {
                    var stream = JsonConvert.DeserializeObject<IStream>(value); 
                    observer.OnNext(stream);                   
                }
                _streams.Subscribe(observer.OnNext);
            });

            BatchStreams = Observable.Create(async (IObserver<List<IStream>> observer) =>
            {
                var values = await Db.HashValuesAsync(_streamsKey);
                var streams = values.Select(s => JsonConvert.DeserializeObject<IStream>(s, JsonSerializerSettings))
                    .ToList();
                observer.OnNext(streams);
                _streams.Subscribe(s => observer.OnNext(new List<IStream> { s }));
            });
        }

        private IDatabase Db => _redis.Database;

        public IObservable<IStream> Streams { get; } 
        public IObservable<List<IStream>> BatchStreams { get; }
        private readonly Subject<IStream> _streams = new Subject<IStream>();

        private async Task UpdateStream(IStream stream)
        {
            await Db.HashSetAsync(_streamsKey, stream.Key.ToString(),
                JsonConvert.SerializeObject(stream, JsonSerializerSettings));
            
            _streams.OnNext(stream);
        }

        private async Task<List<IEvent>> ReadEvents(IEnumerable<RedisValue> commits)
        {
            //Retrieve event data 
            var eventTasks = commits.Select(commit =>
            {
                var partition = commit.ToString().CalculatePartition();

                var hashGetTask = _redis.Database.HashGetAsync(partition, commit);
                return hashGetTask;
            });
            var commitList = await Task.WhenAll(eventTasks).ConfigureAwait(false);

            //Get the events
            var events = commitList.Select(serializedEvent => JsonConvert.DeserializeObject<IEvent>(serializedEvent.ToString(), JsonSerializerSettings))
                .OrderBy(x => x.Timestamp)
                .ToList();
            
            //Get the events
            return events;
        }
        
        public async Task<IEnumerable<IEvent>> ReadStream(IStream stream, long start, int count = -1)
        {
            var commits = await Db.ListRangeAsync(stream.Key.ToString(),start, count == -1 ? -1 : start+count);

            var events = await ReadEvents(commits).ConfigureAwait(false);
            return events;
        }
        
        public async Task<IEnumerable<IEvent>> ReadStreams(IStream[] streams, long[] start)
        {
            Debug.Assert(streams.Length == start.Length);
            
            var commits = new List<RedisValue>();
            for(var i = 0; i < streams.Length; ++i)
                commits.AddRange(await Db.ListRangeAsync(streams[i].Key.ToString(),start[i]));

            var events = await ReadEvents(commits).ConfigureAwait(false);
            return events;
        }

        public async Task WriteStream(IStream stream, IEnumerable<IEvent> events)
        {                        
            foreach (var @event in events)
            {
                var listKey = $"EventStore::{stream.Key.ToString()}";
                var eventId = @event.EventId.ToString(); 

                var serializedEvent = JsonConvert.SerializeObject(@event, JsonSerializerSettings);

                var hashKey = $"EventStore::${eventId.CalculatePartition()}";

                // write the event using hash key
                await Db.HashSetAsync(hashKey, eventId, serializedEvent).ConfigureAwait(false);

                // append the event id to stream 
                var transaction = _redis.Database.CreateTransaction();
                var streamLength = await Db.ListLengthAsync(listKey).ConfigureAwait(false);                
                transaction.ListRightPushAsync(listKey, eventId).ConfigureAwait(false);
                transaction.AddCondition(Condition.ListLengthEqual(listKey, streamLength));

                try
                {
                    //Execute the commit list and publish transactions
                    if (await transaction.ExecuteAsync().ConfigureAwait(false))
                    {
                        try
                        {
                            await _messageQueue.PublishAsync(@event).ConfigureAwait(false);
                            stream.Version++;
                            return;
                        }
                        catch
                        {
                            await Db.ListRemoveAsync(listKey, eventId, -1).ConfigureAwait(false);
                            throw;
                        }
                    }
                }
                catch
                {
                    //The commit list push transaction failed so delete the entry from the event store hash
                    await Db.HashDeleteAsync(hashKey, eventId).ConfigureAwait(false);
                    throw;
                }

                //The commit list push transaction failed so delete the entry from the event store hash
                await Db.HashDeleteAsync(hashKey, eventId).ConfigureAwait(false);

                throw new ConcurrencyException(stream.Key);
            }

            //stream.Version += events.Count(); 
            await UpdateStream(stream);
        }

        public async Task AppendCommand(ICommand command)
        {
            await _redis.Database
                .ListRightPushAsync("commands", JsonConvert.SerializeObject(command, JsonSerializerSettings))
                .ConfigureAwait(false);
        }
    }
}