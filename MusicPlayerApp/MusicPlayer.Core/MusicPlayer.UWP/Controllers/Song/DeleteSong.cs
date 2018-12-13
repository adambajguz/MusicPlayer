using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Song
{
    public class DeleteSong
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
                var song = _uow.SongRepository.Query().Where(x => x.Id == command.ID).FirstOrDefault();
                _uow.SongRepository.Delete(song);
                int i = _uow.SaveChanges();

                await _uow.SaveChangesAsync();
            }
        }
    }
}

