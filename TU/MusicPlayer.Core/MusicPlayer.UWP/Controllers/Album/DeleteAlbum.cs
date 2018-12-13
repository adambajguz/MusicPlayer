using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
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
                var album = _uow.AlbumRepository.Query().Where(x => x.Id == command.ID).FirstOrDefault();
                _uow.AlbumRepository.Delete(album);

                await _uow.SaveChangesAsync();
            }
        }
    }
}

