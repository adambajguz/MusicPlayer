using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.Core.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Album
{
    public class DeleteAlbum
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
                var songs = _uow.SongAlbumRepository.Query().Where(x => x.AlbumId == command.ID).ToList();
                foreach(SongAlbum song in songs)
                {
                    _uow.SongAlbumRepository.Delete(song);
                }
                var album = _uow.AlbumRepository.Query().Where(x => x.Id == command.ID).FirstOrDefault();
                _uow.AlbumRepository.Delete(album);
                int i = _uow.SaveChanges();

                await _uow.SaveChangesAsync();
            }
        }
    }
}

