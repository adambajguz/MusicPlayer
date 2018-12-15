using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Band
{
    public class GetArtists
    {
        public class Query : IQuery
        {
            public int ID;
        }
        public class HandlerList : IQueryHandler<Query, List<Artist.Result>>
        {
            private IUnitOfWork _uow;
            public HandlerList(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<List<Artist.Result>> Handle(Query query)
            {
                var Ids =  _uow.ArtistRepository.Query().Where(x => x.BandId == query.ID).Select(x => x.Id).ToList();
                var result =await _uow.ArtistRepository.Query().Where(x => Ids.Contains(x.Id)).Select(x => new Artist.Result(x)).OrderBy(y => y.Name).ToListAsync();

                return result;
            }

        }
    }
}
