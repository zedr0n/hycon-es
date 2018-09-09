using Hycon.Core.Blocks.Queries;
using Hycon.Infrastructure.Logging;
using Hycon.Infrastructure.Pipes;

namespace Hycon.Console
{
    public class ListBlockListener : HyconBaseListener
    {
        private readonly IBus _bus;
        private readonly ILog _log;

        public ListBlockListener(IBus bus, ILog log)
        {
            _bus = bus;
            _log = log;
        }

        public override void EnterListBlocks(HyconParser.ListBlocksContext context)
        {
            var blocks = _bus.Query(new BlockListQuery());
            foreach(var block in blocks)
                _log.WriteLine(block);
            base.EnterListBlocks(context);
        }
    }
}