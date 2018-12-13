using MusicPlayer.Core.CQRS;
using MusicPlayer.UWP.Controllers.Band;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers
{
    public class BandController : IBandController, IController<Result>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public BandController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Result>> GetAll()
        {
            return await _queryDispatcher.Dispatch<GetBands.Query, List<Result>>(new GetBands.Query());
        }

        public async Task<Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetBand.Query, Result>(new GetBand.Query() { ID = id });
        }

        public async Task Create(string name, DateTime creationDate, DateTime? endDate, string description)
        {
            await _commandDispatcher.Dispatch<CreateBand.Command>(new CreateBand.Command
            {
                _data = new CreateBand.Data(name, creationDate, endDate, description)

            });

        }

        public async Task Delete(int id)
        {
            await _commandDispatcher.Dispatch<DeleteBand.Command>(new DeleteBand.Command
            {
                ID = id
            });
        }
    }
}
