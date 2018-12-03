using MusicPlayer.Core.CQRS;
using MusicPlayer.Service.Controllers.Image;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers
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

        public async Task<List<Image.GetImages.Result>> GetImages()
        {
            return await _queryDispatcher.Dispatch<GetImages.Query, List<GetImages.Result>>(new GetImages.Query());
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
