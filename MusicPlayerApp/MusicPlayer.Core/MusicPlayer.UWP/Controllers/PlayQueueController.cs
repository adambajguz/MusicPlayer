using MusicPlayer.Core.CQRS;
using MusicPlayer.UWP.Controllers.PlayQueue;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers
{
    public class PlayQueueController : IPlayQueueController, IController<Result>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public PlayQueueController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Result>> GetAll()
        {
            return await _queryDispatcher.Dispatch<GetPlayQueues.Query, List<Result>>(new GetPlayQueues.Query());
        }

        public async Task<Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetPlayQueue.Query, Result>(new GetPlayQueue.Query() { ID = id });
        }

        public async Task<Song.Result> RandomSong()
        {
            return await _queryDispatcher.Dispatch<RandomSong.Query, Song.Result>(new RandomSong.Query());
        }

        public async Task Create(int songId)
        {
            await _commandDispatcher.Dispatch<CreatePlayQueue.Command>(new CreatePlayQueue.Command
            {
                _data = new CreatePlayQueue.Data(songId)

            });

        }

        public async Task Delete(int id)
        {
            await _commandDispatcher.Dispatch<DeletePlayQueue.Command>(new DeletePlayQueue.Command
            {
                ID = id
            });
        }
    }
}
