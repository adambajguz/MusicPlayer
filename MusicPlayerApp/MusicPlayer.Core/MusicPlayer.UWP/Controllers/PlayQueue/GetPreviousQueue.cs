using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.PlayQueue
{
    public class GetPreviousQueue
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
                var result = await _uow.PlayQueueRepository.Query().OrderByDescending(x => x.Id).Skip(1).Select(x => new Result(x)).FirstAsync();
                return result;
            }
        }
    }
}
