using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MusicPlayer.UWP.Controllers.Song
{
    public class GetAlbums
    {
        public class Query : IQuery
        {
            public int ID;
        }

        public class Handler : IQueryHandler<Query, List<Album.Result>>
        {
            private IUnitOfWork _uow;
            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<List<Album.Result>> Handle(Query query)
            {
                var artistId = _uow.SongAlbumRepository.Query().Where(x => x.SongId == query.ID).Select(x => x.AlbumId).ToList();
                var result = await _uow.AlbumRepository.Query().Where(x => artistId.Contains(x.Id)).Select(x => new Album.Result(x)).ToListAsync();
                return result;
            }
        }
    }
}
