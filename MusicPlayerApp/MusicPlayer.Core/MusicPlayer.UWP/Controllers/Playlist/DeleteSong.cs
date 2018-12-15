using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Playlist
{
    public class DeleteSong
    {
        public class Command : ICommand
        {
            public int SongId;
            public int PlaylistId;
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
                var songPlaylist = _uow.SongPlaylistRepository.Query().Where(x => (x.SongId == command.SongId & x.PlaylistId == command.PlaylistId)).FirstOrDefault();
                _uow.SongPlaylistRepository.Delete(songPlaylist);
                int i = _uow.SaveChanges();

                await _uow.SaveChangesAsync();
            }
        }
    }
}

