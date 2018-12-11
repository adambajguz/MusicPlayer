using MusicPlayer.Core.CQRS;
using MusicPlayer.Service.Controllers.Playlist;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers
{
    public class PlaylistController
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public PlaylistController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Result>> GetBands()
        {
            return await _queryDispatcher.Dispatch<GetPlaylists.Query, List<Result>>(new GetPlaylists.Query());
        }

        public async Task<Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetPlaylist.Query, Result>(new GetPlaylist.Query() { ID = id });
        }

        public async Task Create(string name, string description)
        {
            await _commandDispatcher.Dispatch<CreatePlaylist.Command>(new CreatePlaylist.Command
            {
                _data = new CreatePlaylist.Data(name, description)

            });

        }

        public async Task Delete(int id)
        {
            await _commandDispatcher.Dispatch<DeletePlaylist.Command>(new DeletePlaylist.Command
            {
                ID = id
            });
        }
    }
}
