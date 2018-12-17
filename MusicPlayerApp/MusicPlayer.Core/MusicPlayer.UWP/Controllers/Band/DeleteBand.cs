using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Band
{
    public class DeleteBand
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
                var artists = _uow.ArtistRepository.Query().Where(x => x.BandId==command.ID).ToList();
                foreach(Artist.Result artist in artists)
                {
                    artist.BandId = null;
                }

                var band = _uow.BandRepository.Query().Where(x => x.Id == command.ID).FirstOrDefault();
                _uow.BandRepository.Delete(band);

                int i = _uow.SaveChanges();

                await _uow.SaveChangesAsync();
            }
        }
    }
}

