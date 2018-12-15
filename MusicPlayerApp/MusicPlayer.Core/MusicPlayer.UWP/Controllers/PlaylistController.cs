using MusicPlayer.Core.CQRS;
using MusicPlayer.UWP.Controllers.Playlist;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers
{
    public class PlaylistController : IPlaylistController, IController<Result>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public PlaylistController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Result>> GetAll()
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

        public async Task AddSong(int playlistId, int songId)
        {
            await _commandDispatcher.Dispatch<AddSong.Command>(new AddSong.Command
            {
                _data = new AddSong.Data(playlistId, songId)

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
