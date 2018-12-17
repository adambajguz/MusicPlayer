using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MusicPlayer.UWP.Controllers.Song
{
    public class GetArtists
    {
        public class Query : IQuery
        {
            public int ID;
        }

        public class Handler : IQueryHandler<Query, List<Artist.Result>>
        {
            private IUnitOfWork _uow;
            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<List<Artist.Result>> Handle(Query query)
            {
                var artistId = _uow.SongArtistRepository.Query().Where(x => x.SongId == query.ID).Select(x => x.ArtistId).ToList();
                var result = await _uow.ArtistRepository.Query().Where(x => artistId.Contains(x.Id)).Select(x => new Artist.Result(x)).ToListAsync();
                return result;
            }
        }
    }
}
