using System;
using System.Collections.Generic;
using Hycon.Core.Blocks.Commands;
using Hycon.Core.Blocks.Queries;
using Hycon.Infrastructure.Pipes;

namespace Hycon.Console
{
    public class PutBlockListener : HyconBaseListener 
    {
        private readonly IBus _bus;

        public PutBlockListener(IBus bus)
        {
            _bus = bus;
        }

        public override void EnterPutBlock(HyconParser.PutBlockContext context)
        {
            var hash = context.hash().GetText();
            var previousHash = context.previousHash().GetText();

            var id = _bus.Query(new BlockQuery(hash));
            if (id == Guid.Empty)
                id = Guid.NewGuid();
            
            var command = new PutBlockCommand(id, hash, new List<string> {previousHash});
            _bus.Command(command);

            base.EnterPutBlock(context);
        }
    }
}