using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

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
                var genre = _uow.GenreRepository.Query().Where(x => x.Id == command.ID).FirstOrDefault();
                _uow.GenreRepository.Delete(genre);

                await _uow.SaveChangesAsync();
            }
        }
    }
}

