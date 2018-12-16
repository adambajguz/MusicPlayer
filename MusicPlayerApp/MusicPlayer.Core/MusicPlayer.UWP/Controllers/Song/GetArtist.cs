using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Song
{
    public class GetArtist
    {
        public class Query : IQuery
        {
            public int ID;
        }

        public class Handler : IQueryHandler<Query, Artist.Result>
        {
            private IUnitOfWork _uow;
            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Artist.Result> Handle(Query query)
            {
                var artistId = _uow.SongArtistRepository.Query().Where(x => x.SongId == query.ID).Select(x => x.ArtistId).First();
                var result = await _uow.ArtistRepository.Query().Where(x => x.Id == artistId).Select(x => new Artist.Result(x)).FirstOrDefaultAsync();
                return result;
            }
        }
    }
}
