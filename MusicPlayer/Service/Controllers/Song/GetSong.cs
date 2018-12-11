using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers.Song
{
    public class GetSong
    {
        public class Query : IQuery
        {
            public int ID;
        }

        public class Handler : IQueryHandler<Query, Result>
        {
            private IUnitOfWork _uow;
            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Result> Handle(Query query)
            {
                var result = await _uow.SongRepository.Query().Where(x => x.Id == query.ID).Select(x => new Result(x)).FirstOrDefaultAsync();
                return result;
            }
        }
    }
}
