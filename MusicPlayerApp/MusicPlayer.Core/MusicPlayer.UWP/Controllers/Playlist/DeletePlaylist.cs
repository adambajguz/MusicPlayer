using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.Core.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Playlist
{
    public class DeletePlaylist
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
                var songs = _uow.SongPlaylistRepository.Query().Where(x => x.PlaylistId == command.ID).ToList();
                foreach(SongPlaylist song in songs)
                {
                    _uow.SongPlaylistRepository.Delete(song);
                }
                var playlist = _uow.PlaylistRepository.Query().Where(x => x.Id == command.ID).FirstOrDefault();
                _uow.PlaylistRepository.Delete(playlist);
                int i = _uow.SaveChanges();

                await _uow.SaveChangesAsync();
            }
        }
    }
}

