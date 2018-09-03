using System.Threading.Tasks;
using Hycon.Interfaces.Domain;

namespace Hycon.Core.Blocks.Commands
{
    public class CreateBlockCommandHandler : ICommandHandler<CreateBlockCommand>
    {
        private readonly IDomainRepository _repository;

        public CreateBlockCommandHandler(IDomainRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateBlockCommand command)
        {
            var block = new Block(command.BlockId, command.Hash, command.PreviousHash );
            await _repository.Save(block);
        }
    }
}