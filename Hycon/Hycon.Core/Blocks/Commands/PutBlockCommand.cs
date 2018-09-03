using System;
using System.Collections.Generic;
using Hycon.Infrastructure.Domain;

namespace Hycon.Core.Blocks.Commands
{
    public class PutBlockCommand : Command
    {
        public PutBlockCommand(Guid blockId, string hash, List<string> previousHash)
        {
            BlockId = blockId;
            Hash = hash;
            PreviousHash = previousHash;
        }

        public Guid BlockId { get; }
        public string Hash { get; }
        public List<string> PreviousHash { get; }
    }
}