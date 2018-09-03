using System;
using System.Collections.Generic;
using Hycon.Core.Blocks.Commands;
using Hycon.Infrastructure.Bus;

namespace Hycon.Console
{
    public class CreateBlockListener : HyconBaseListener 
    {
        private readonly IBus _bus;

        public CreateBlockListener(IBus bus)
        {
            _bus = bus;
        }

        public override void EnterCreateBlock(HyconParser.CreateBlockContext context)
        {
            var hash = context.hash().GetText();
            var previousHash = context.previousHash().GetText();
            var id = Guid.NewGuid();

            var command = new CreateBlockCommand(id, hash, new List<string> {previousHash});
            _bus.Command(command);

            base.EnterCreateBlock(context);
        }
    }
}