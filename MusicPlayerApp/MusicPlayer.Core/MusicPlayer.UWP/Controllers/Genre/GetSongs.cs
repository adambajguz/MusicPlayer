using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Genre
{
    public class GetSongs
    {
        public class Query : IQuery
        {
            public int ID;
        }
        public class HandlerList : IQueryHandler<Query, List<Song.Result>>
        {
            private IUnitOfWork _uow;
            public HandlerList(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<List<Song.Result>> Handle(Query query)
            {
                var result = await _uow.SongRepository.Query().Where(x => x.GenreId==query.ID).Select(x => new Song.Result(x)).ToListAsync();

                return result;
            }

        }
    }
}
