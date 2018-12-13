using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.PlayQueue
{
    public class DeletePlayQueue
    {
        public class Command : ICommand
        {
            public int ID;
        }

        public class Handler : ICommandHandler<Command>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task Execute(Command command)
            {
                var playQueue = _uow.PlayQueueRepository.Query().Where(x => x.Id == command.ID).FirstOrDefault();
                _uow.PlayQueueRepository.Delete(playQueue);
                int i = _uow.SaveChanges();

                await _uow.SaveChangesAsync();
            }
        }
    }
}

