using System.Collections.Generic;
using System.Threading.Tasks;
using Hycon.Core.Blocks.Projections;
using Hycon.Interfaces.Domain;

namespace Hycon.Core.Blocks.Queries
{
    public class BlockListQueryHandler : IQueryHandler<BlockListQuery, IEnumerable<string>>
    {
        private readonly BlockListProjection _projection;

        public BlockListQueryHandler(BlockListProjection projection)
        {
            _projection = projection;
        }

        public IEnumerable<string> Handle(BlockListQuery query)
        {
            return _projection.GetAll();
        }

        public Task<IEnumerable<string>> HandleAsync(BlockListQuery query)
        {
            throw new System.NotImplementedException();
        }
    }
}