using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Artist
{
    public class DeleteSong
    {
        public class Command : ICommand
        {
            public int SongId;
            public int ArtistId;
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
                var songArtist = _uow.SongArtistRepository.Query().Where(x => (x.SongId == command.SongId & x.ArtistId == command.ArtistId)).FirstOrDefault();
                _uow.SongArtistRepository.Delete(songArtist);
                int i = _uow.SaveChanges();

                await _uow.SaveChangesAsync();
            }
        }
    }
}

