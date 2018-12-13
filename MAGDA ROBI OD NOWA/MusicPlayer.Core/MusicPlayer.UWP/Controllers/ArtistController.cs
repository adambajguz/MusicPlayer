using MusicPlayer.Core.CQRS;
using MusicPlayer.UWP.Controllers.Artist;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers
{
    public class ArtistController
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public ArtistController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Result>> GetBands()
        {
            return await _queryDispatcher.Dispatch<GetArtists.Query, List<Result>>(new GetArtists.Query());
        }

        public async Task<Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetArtist.Query, Result>(new GetArtist.Query() { ID = id });
        }

        public async Task Create(string name, string surname, string pseudonym, DateTime birthdate, string description)
        {
            await _commandDispatcher.Dispatch<CreateArtist.Command>(new CreateArtist.Command
            {
                _data = new CreateArtist.Data(name, surname, pseudonym, birthdate, description)

            });

        }

        public async Task Delete(int id)
        {
            await _commandDispatcher.Dispatch<DeleteArtist.Command>(new DeleteArtist.Command
            {
                ID = id
            });
        }
    }
}
