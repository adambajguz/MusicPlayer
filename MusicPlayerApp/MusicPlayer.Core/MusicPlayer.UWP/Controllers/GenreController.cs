using MusicPlayer.Core.CQRS;
using MusicPlayer.UWP.Controllers.Genre;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers
{
    public class GenreController : IGenreController, IController<Result>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public GenreController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Result>> GetAll()
        {
            return await _queryDispatcher.Dispatch<GetGenres.Query, List<Result>>(new GetGenres.Query());
        }

        public async Task<List<Result>> GetAllDescending()
        {
            return await _queryDispatcher.Dispatch<GetGenresDescending.Query, List<Result>>(new GetGenresDescending.Query());
        }

        public async Task<List<Result>> Search(string name)
        {
            return await _queryDispatcher.Dispatch<SearchGenres.Query, List<Result>>(new SearchGenres.Query() { Name = name });
        }

        public async Task<Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetGenre.Query, Result>(new GetGenre.Query() { ID = id });
        }

        public async Task<int> SongQuantity(int genreId)
        {
            return await _queryDispatcher.Dispatch<SongQuantity.Query, int>(new SongQuantity.Query() { ID = genreId });
        }

        public async Task<List<Song.Result>> GetSongs(int genreId)
        {
            return await _queryDispatcher.Dispatch<GetSongs.Query, List<Song.Result>>(new GetSongs.Query() { ID = genreId });
        }

        public async Task Create(string name, string description)
        {
            await _commandDispatcher.Dispatch<CreateGenre.Command>(new CreateGenre.Command
            {
                _data = new CreateGenre.Data(name, description)

            });

        }

        public async Task Delete(int id)
        {
            await _commandDispatcher.Dispatch<DeleteGenre.Command>(new DeleteGenre.Command
            {
                ID = id
            });
        }
    }
}
