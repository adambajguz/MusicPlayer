using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.PlayQueue
{
    public class RandomSong
    {
        public class Query : IQuery
        {
        }

        public class Handler : IQueryHandler<Query, Song.Result>
        {
            private IUnitOfWork _uow;
            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Song.Result> Handle(Query query)
            {
                Random random = new Random();
                var result = await _uow.SongRepository.Query().OrderBy(x => random.Next()).Select(x => new Song.Result(x)).Take(1).FirstOrDefaultAsync();
                return result;
            }
        }
    }
}
