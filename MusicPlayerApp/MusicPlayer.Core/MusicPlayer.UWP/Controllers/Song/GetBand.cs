using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Song
{
    public class GetBand
    {
        public class Query : IQuery
        {
            public int ID;
        }

        public class Handler : IQueryHandler<Query, Band.Result>
        {
            private IUnitOfWork _uow;
            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Band.Result> Handle(Query query)
            {
                var bandId = _uow.ArtistRepository.Query().Where(x => x.Id == query.ID).Select(x => x.BandId).First();
                var result = await _uow.BandRepository.Query().Where(x => x.Id == bandId).Select(x => new Band.Result(x)).FirstOrDefaultAsync();
                return result;
            }
        }
    }
}
