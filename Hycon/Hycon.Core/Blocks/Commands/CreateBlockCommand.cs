using System;
using System.Collections.Generic;
using Hycon.Infrastructure.Domain;

namespace Hycon.Core.Blocks.Commands
{
    public class CreateBlockCommand : Command
    {
        public Guid BlockId { get; }
        public string Hash { get; }
        public List<string> PreviousHash { get; }
        
        public CreateBlockCommand(Guid blockId,string hash, List<string> previousHash) 
        {
            Hash = hash;
            PreviousHash = previousHash;
            BlockId = blockId;
        }
    }
}