using System;
using System.Collections.Generic;
using Hycon.Infrastructure;
using NodaTime;

namespace Hycon.Core.Blocks.Events
{
    public class BlockCreated : EventBase
    {
        public Guid BlockId { get; }
        public string Hash { get; }
        public List<string> PreviousHash { get; }
        
        public BlockCreated(Guid blockId, string hash, List<string> previousHash)
        {
            BlockId = blockId;
            Hash = hash;
            PreviousHash = previousHash;
        }
    }
}