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

        public async Task<List<Result>> GetAllDescending()
        {
            return await _queryDispatcher.Dispatch<GetPlaylistsDescending.Query, List<Result>>(new GetPlaylistsDescending.Query());
        }

        public async Task<List<Result>> GetAllByDate()
        {
            return await _queryDispatcher.Dispatch<GetPlaylistsByDate.Query, List<Result>>(new GetPlaylistsByDate.Query());
        }

        public async Task<List<Result>> GetAllByDateDescending()
        {
            return await _queryDispatcher.Dispatch<GetPlaylistsByDateDescending.Query, List<Result>>(new GetPlaylistsByDateDescending.Query());
        }

        public async Task<List<Song.Result>> GetSongs(int playlistId)
        {
            return await _queryDispatcher.Dispatch<GetSongs.Query, List<Song.Result>>(new GetSongs.Query() { ID = playlistId });
        }

        public async Task<List<Result>> Search(string name)
        {
            return await _queryDispatcher.Dispatch<SearchPlaylists.Query, List<Result>>(new SearchPlaylists.Query() { Name = name });
        }

        public async Task<Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetPlaylist.Query, Result>(new GetPlaylist.Query() { ID = id });
        }

        public async Task<int> Create(string name, string description)
        {
            return await _commandDispatcher.Dispatch<CreatePlaylist.Command, int>(new CreatePlaylist.Command
            {
                _data = new CreatePlaylist.Data(name, description)

            });

        }

        public async Task Update(int id, string name, string description)
        {
            await _commandDispatcher.Dispatch<UpdatePlaylist.Command>(new UpdatePlaylist.Command
            {
                ID = id,
                Name = name,
                Description = description

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

        public async Task DeleteSong(int songId, int playlistId)
        {
            await _commandDispatcher.Dispatch<DeleteSong.Command>(new DeleteSong.Command
            {
                SongId = songId,
                PlaylistId = playlistId
            });
        }
    }
}
