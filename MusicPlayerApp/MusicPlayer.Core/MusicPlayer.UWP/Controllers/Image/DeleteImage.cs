using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Image
{
    public class DeleteImage
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
                var image = _uow.ImageRepository.Query().Where(x => x.Id == command.ID).FirstOrDefault();
                _uow.ImageRepository.Delete(image);
                int i = _uow.SaveChanges();

                await _uow.SaveChangesAsync();
            }
        }
    }
}

