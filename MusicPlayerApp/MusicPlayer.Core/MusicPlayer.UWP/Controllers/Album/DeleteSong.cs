using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Album
{
    public class DeleteSong
    {
        public class Command : ICommand
        {
            public int SongId;
            public int AlbumId;
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
                var songAlbum = _uow.SongAlbumRepository.Query().Where(x => (x.SongId == command.SongId & x.AlbumId==command.AlbumId)).FirstOrDefault();
                _uow.SongAlbumRepository.Delete(songAlbum);
                int i = _uow.SaveChanges();

                await _uow.SaveChangesAsync();
            }
        }
    }
}

