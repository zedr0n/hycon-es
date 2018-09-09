using System;
using System.Threading.Tasks;
using Hycon.Core.Blocks.Projections;
using Hycon.Interfaces.Domain;

namespace Hycon.Core.Blocks.Queries
{
    public class BlockQueryHandler : IQueryHandler<BlockQuery,Guid>
    {
        private readonly BlockListProjection _projection;

        public BlockQueryHandler(BlockListProjection projection)
        {
            _projection = projection;
        }

        public Guid Handle(BlockQuery query)
        {
            var id = _projection.GetByHash(query.Hash);
            return id; 
        }

        public async Task<Guid> HandleAsync(BlockQuery query)
        {
            throw new NotImplementedException();
        }
    }
}