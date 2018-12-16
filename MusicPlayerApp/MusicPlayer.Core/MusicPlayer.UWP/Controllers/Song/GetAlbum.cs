using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Song
{
    public class GetAlbum
    {
        public class Query : IQuery
        {
            public int ID;
        }

        public class Handler : IQueryHandler<Query, Album.Result>
        {
            private IUnitOfWork _uow;
            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Album.Result> Handle(Query query)
            {
                var albumId = _uow.SongAlbumRepository.Query().Where(x => x.SongId == query.ID).Select(x => x.AlbumId).First();
                var result = await _uow.AlbumRepository.Query().Where(x => x.Id == albumId).Select(x => new Album.Result(x)).FirstOrDefaultAsync();
                return result;
            }
        }
    }
}
