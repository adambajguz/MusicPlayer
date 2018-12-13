using MusicPlayer.Core.CQRS;
using MusicPlayer.Service.Controllers.PlayQueue;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers
{
    public class PlayQueueController
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public PlayQueueController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Result>> GetPlayQueues()
        {
            return await _queryDispatcher.Dispatch<GetPlayQueues.Query, List<Result>>(new GetPlayQueues.Query());
        }

        public async Task<Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetPlayQueue.Query, Result>(new GetPlayQueue.Query() { ID = id });
        }

        public async Task Create(string name, string description)
        {
            await _commandDispatcher.Dispatch<CreatePlayQueue.Command>(new CreatePlayQueue.Command
            {
                _data = new CreatePlayQueue.Data()

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
