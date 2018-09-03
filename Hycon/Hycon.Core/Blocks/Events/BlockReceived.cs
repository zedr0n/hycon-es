using System;
using Hycon.Infrastructure;

namespace Hycon.Core.Blocks.Events
{
    public class BlockReceived : EventBase
    {
        public Guid BlockId { get; }
        
        public BlockReceived(Guid blockId)
        {
            BlockId = blockId;
        }
    }
}