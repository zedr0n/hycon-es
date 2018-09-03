using System;
using System.Collections.Generic;
using Hycon.Core.Blocks.Events;
using Hycon.Infrastructure.Domain;
using Hycon.Interfaces;

namespace Hycon.Core.Blocks
{
    public class Block : AggregateRoot
    {
        private string _hash;
        private List<string> _previousHash = new List<string>();
        
        public Block() {}

        public Block(Guid id, string hash, List<string> previousHash)
        {
            Id = id;
            When(new BlockCreated(Id, hash, previousHash));
        }

        public void PutBlock()
        {
            When( new BlockReceived(Id) ); 
        }
        
        protected void ApplyEvent(BlockReceived e) {}

        protected void ApplyEvent(BlockCreated e)
        {
            _hash = e.Hash;
            _previousHash = e.PreviousHash;
        }

        protected override void ApplyEvent(IEvent e)
        {
            ApplyEvent((dynamic) e);
        }
    }
}