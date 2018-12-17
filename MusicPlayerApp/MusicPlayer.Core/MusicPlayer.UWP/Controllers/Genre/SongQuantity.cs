using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Genre
{
    public class SongQuantity
    {
        public class Query : IQuery
        {
            public int ID;
        }

        public class Handler : IQueryHandler<Query, int>
        {
            private IUnitOfWork _uow;
            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<int> Handle(Query query)
            {
                var quantity =await _uow.SongRepository.Query().Where(x => x.GenreId == query.ID).CountAsync();
                return quantity;
            }
        }
    }
}
