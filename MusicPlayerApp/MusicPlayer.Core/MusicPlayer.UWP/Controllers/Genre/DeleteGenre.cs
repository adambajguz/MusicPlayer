using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;
using MusicPlayer.Core.Entities;

namespace MusicPlayer.UWP.Controllers.Genre
{
    public class DeleteGenre
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
                var songs = _uow.SongRepository.Query().Where(x => x.GenreId == command.ID).ToList();
                foreach(Core.Entities.Song song in songs)
                {
                    _uow.SongRepository.Delete(song);
                }

                var genre = _uow.GenreRepository.Query().Where(x => x.Id == command.ID).FirstOrDefault();
                _uow.GenreRepository.Delete(genre);
                int i = _uow.SaveChanges();

                await _uow.SaveChangesAsync();
            }
        }
    }
}

