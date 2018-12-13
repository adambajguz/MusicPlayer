using MusicPlayer.Core.CQRS;
using MusicPlayer.UWP.Controllers.Album;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers
{
    public class AlbumController
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public AlbumController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Result>> GetBands()
        {
            return await _queryDispatcher.Dispatch<GetAlbums.Query, List<Result>>(new GetAlbums.Query());
        }

        public async Task<Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetAlbum.Query, Result>(new GetAlbum.Query() { ID = id });
        }

        public async Task Create(string title, string description, DateTime publicationDate)
        {
            await _commandDispatcher.Dispatch<CreateAlbum.Command>(new CreateAlbum.Command
            {
                _data = new CreateAlbum.Data(title, description, publicationDate)

            });

        }

        public async Task Delete(int id)
        {
            await _commandDispatcher.Dispatch<DeleteAlbum.Command>(new DeleteAlbum.Command
            {
                ID = id
            });
        }
    }
}
