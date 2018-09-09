using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Hycon.Core.Blocks.Events;
using Hycon.Infrastructure.Projections;
using Hycon.Interfaces;
using Hycon.Interfaces.EventStore;

namespace Hycon.Core.Blocks.Projections
{
    public class BlockListProjection : Projection
    {
        private readonly ConcurrentDictionary<string, Guid> _blocks = new ConcurrentDictionary<string, Guid>(); 
        
        public BlockListProjection(IEventStore eventStore) : base(eventStore)
        {
            Register<BlockCreated>(When);
            
            eventStore.Streams
                .Where(s => s.EventSourcedType == typeof(Block))
                .Select(s => new List<IStream> {s})
                .Subscribe(async s => await Update(s));
        }

        public Guid GetByHash(string hash)
        {
            return _blocks.TryGetValue(hash, out var id) ? id : Guid.Empty;
        }

        public List<string> GetAll()
        {
            return _blocks.Keys.ToList();
        }

        private void When(BlockCreated e)
        {
            _blocks[e.Hash] = e.BlockId;
        }
    }
}