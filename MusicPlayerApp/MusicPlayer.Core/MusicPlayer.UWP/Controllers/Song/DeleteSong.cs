using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.Core.Entities;
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


            //nie wiem co tu zrobic w bazie danych z sql mamy ze SongId w SongAlbum nie moze byc nullem
            public async Task Execute(Command command)
            {
                var songsplaylist = _uow.SongPlaylistRepository.Query().Where(x => x.SongId == command.ID).ToList();
                foreach (SongPlaylist songg in songsplaylist)
                {
                    _uow.SongPlaylistRepository.Delete(songg);
                }

                var songsartist = _uow.SongArtistRepository.Query().Where(x => x.SongId == command.ID).ToList();
                foreach (SongArtist songg in songsartist)
                {
                    _uow.SongArtistRepository.Delete(songg);
                }

                var songsalbum = _uow.SongAlbumRepository.Query().Where(x => x.SongId == command.ID).ToList();
                foreach (SongAlbum songg in songsalbum)
                {
                    _uow.SongAlbumRepository.Delete(songg);
                }

                var song = _uow.SongRepository.Query().Where(x => x.Id == command.ID).FirstOrDefault();
                _uow.SongRepository.Delete(song);
                int i = _uow.SaveChanges();

                await _uow.SaveChangesAsync();
            }
        }
    }
}

