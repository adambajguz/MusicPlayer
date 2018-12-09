using EShop.Controllers.Band;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Service.Controllers.Band;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers
{
    public class BandController
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public BandController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Band.GetBands.Result>> GetBands()
        {
            return await _queryDispatcher.Dispatch<GetBands.Query, List<GetBands.Result>>(new GetBands.Query());
        }

        public async Task<Band.GetBand.Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetBand.Query, GetBand.Result>(new GetBand.Query() { ID = id });
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
                _data = new DeleteBand.Data(id)
            });
        }
    }
}
