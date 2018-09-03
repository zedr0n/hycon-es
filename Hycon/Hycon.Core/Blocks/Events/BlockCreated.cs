using System;
using System.Collections.Generic;
using Hycon.Infrastructure;
using NodaTime;

namespace Hycon.Core.Blocks.Events
{
    public class BlockCreated : EventBase
    {
        public string Hash { get; }
        public List<string> PreviousHash { get; }
        
        public BlockCreated(string hash, List<string> previousHash) 
        {
            Hash = hash;
            PreviousHash = previousHash;
        }
    }
}