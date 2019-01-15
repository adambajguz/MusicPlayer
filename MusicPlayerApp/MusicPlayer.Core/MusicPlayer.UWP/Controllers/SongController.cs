using MusicPlayer.Core.CQRS;
using MusicPlayer.UWP.Controllers.Song;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers
{
    public class SongController : ISongController, IController<Result>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public SongController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Result>> GetAll()
        {
            return await _queryDispatcher.Dispatch<GetSongs.Query, List<Result>>(new GetSongs.Query());
        }

        public async Task<List<Result>> GetAllDescending()
        {
            return await _queryDispatcher.Dispatch<GetSongsDescending.Query, List<Result>>(new GetSongsDescending.Query());
        }

        public async Task<List<Result>> GetAllByDate()
        {
            return await _queryDispatcher.Dispatch<GetSongsByDate.Query, List<Result>>(new GetSongsByDate.Query());
        }

        public async Task<List<Result>> GetAllByDateDescending()
        {
            return await _queryDispatcher.Dispatch<GetSongsByDateDescending.Query, List<Result>>(new GetSongsByDateDescending.Query());
        }

        public async Task<List<Result>> GetAllByLength()
        {
            return await _queryDispatcher.Dispatch<GetSongsByLength.Query, List<Result>>(new GetSongsByLength.Query());
        }

        public async Task<List<Result>> GetAllByLengthDescending()
        {
            return await _queryDispatcher.Dispatch<GetSongsByLengthDescending.Query, List<Result>>(new GetSongsByLengthDescending.Query());
        }

        public async Task<List<Result>> GetAllByPlayTimes()
        {
            return await _queryDispatcher.Dispatch<GetSongsByPlayTimes.Query, List<Result>>(new GetSongsByPlayTimes.Query());
        }

        public async Task<List<Result>> GetAllByPlayTimesDescending()
        {
            return await _queryDispatcher.Dispatch<GetSongsByPlayTimesDescending.Query, List<Result>>(new GetSongsByPlayTimesDescending.Query());
        }

        public async Task<List<Result>> GetAllByScore()
        {
            return await _queryDispatcher.Dispatch<GetSongsByScore.Query, List<Result>>(new GetSongsByScore.Query());
        }

        public async Task<List<Result>> GetAllByScoreDescending()
        {
            return await _queryDispatcher.Dispatch<GetSongsByScoreDescending.Query, List<Result>>(new GetSongsByScoreDescending.Query());
        }

        public async Task<List<Result>> SearchSongs(string name)
        {
            return await _queryDispatcher.Dispatch<SearchSongs.Query, List<Result>>(new SearchSongs.Query() { Name=name });
        }

        public async Task<Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetSong.Query, Result>(new GetSong.Query() { ID = id });
        }

        public async Task<List<Artist.Result>> GetArtists(int songId)
        {
            return await _queryDispatcher.Dispatch<GetArtists.Query, List<Artist.Result>>(new GetArtists.Query() { ID = songId });
        }

        public async Task<Album.Result> GetAlbum(int songId)
        {
            return await _queryDispatcher.Dispatch<GetAlbum.Query, Album.Result>(new GetAlbum.Query() { ID = songId });
        }

        public async Task<Band.Result> GetBand(int artistId)
        {
            return await _queryDispatcher.Dispatch<GetBand.Query, Band.Result>(new GetBand.Query() { ID = artistId });
        }

        public async Task Create(int score, string title, DateTime creationDate, string filePath, int? imageId, int genreId)
        {
            await _commandDispatcher.Dispatch<CreateSong.Command>(new CreateSong.Command
            {
                _data = new CreateSong.Data(score, title, creationDate, filePath, imageId,genreId)

            });

        }

        public async Task Update(int id, int score, string title, DateTime creationDate, string filePath, int? imageId, int genreId)
        {
            await _commandDispatcher.Dispatch<UpdateSong.Command>(new UpdateSong.Command
            {
                ID = id,
                Score=score,
                Title=title,
                CreationDate=creationDate,
                FilePath=filePath,
                ImageId=imageId,
                GenreId=genreId

            });

        }

        public async Task IncreasePlayTimes(int id)
        {
            await _commandDispatcher.Dispatch<IncreasePlayTimes.Command>(new IncreasePlayTimes.Command
            {
                ID = id
            });
        }

        public async Task SetScore(int id, int score)
        {
            await _commandDispatcher.Dispatch<SetScore.Command>(new SetScore.Command
            {
                ID = id,
                Score=score
            });
        }

        public async Task Delete(int id)
        {
            await _commandDispatcher.Dispatch<DeleteSong.Command>(new DeleteSong.Command
            {
                ID = id
            });
        }
    }
}
