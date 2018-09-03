using System.Threading.Tasks;
using Hycon.Interfaces.Domain;

namespace Hycon.Core.Blocks.Commands
{
    public class PutBlockCommandHandler : ICommandHandler<PutBlockCommand>
    {
        private readonly IDomainRepository _repository;

        public PutBlockCommandHandler(IDomainRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(PutBlockCommand command)
        {
            var blockId = command.BlockId;
            var block = await _repository.Find<Block>(blockId) ?? new Block(blockId, command.Hash, command.PreviousHash);

            block.PutBlock();
            await _repository.Save(block);
        }
    }
}