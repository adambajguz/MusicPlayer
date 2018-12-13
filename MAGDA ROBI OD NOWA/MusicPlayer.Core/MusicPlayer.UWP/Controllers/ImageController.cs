using MusicPlayer.Core.CQRS;
using MusicPlayer.UWP.Controllers.Image;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers
{
    public class ImageController
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public ImageController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Result>> GetImages()
        {
            return await _queryDispatcher.Dispatch<GetImages.Query, List<Result>>(new GetImages.Query());
        }

        public async Task<Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetImage.Query, Result>(new GetImage.Query() { ID = id });
        }

        public async Task Create(string filePath)
        {
            await _commandDispatcher.Dispatch<CreateImage.Command>(new CreateImage.Command
            {
                _data = new CreateImage.Data(filePath)

            });

        }
    }
}
