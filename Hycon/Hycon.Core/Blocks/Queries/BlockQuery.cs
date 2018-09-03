using System;
using Hycon.Interfaces.Domain;

namespace Hycon.Core.Blocks.Queries
{
    public class BlockQuery : IQuery<Guid>
    {
        public BlockQuery(string hash)
        {
            Hash = hash;
        }

        public string Hash { get; }
    }
}