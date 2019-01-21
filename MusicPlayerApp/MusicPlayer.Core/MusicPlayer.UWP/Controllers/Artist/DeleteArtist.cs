using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.Core.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Artist
{
    public class DeleteArtist
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
                var songs = _uow.SongArtistRepository.Query().Where(x => x.ArtistId == command.ID).ToList();
                foreach (SongArtist song in songs)
                {
                    _uow.SongArtistRepository.Delete(song);
                }

                //var artist = _uow.ArtistRepository.Query().Where(x => x.Id == command.ID).FirstOrDefault();
                //_uow.ArtistRepository.Delete(artist);
                int i = _uow.SaveChanges();

                await _uow.SaveChangesAsync();
            }
        }
    }
}

