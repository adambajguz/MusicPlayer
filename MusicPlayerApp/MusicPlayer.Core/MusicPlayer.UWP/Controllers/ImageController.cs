using MusicPlayer.Core.CQRS;
using MusicPlayer.UWP.Controllers.Image;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers
{
    public class ImageController : IImageController, IController<Result>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public ImageController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Result>> GetAll()
        {
            return await _queryDispatcher.Dispatch<GetImages.Query, List<Result>>(new GetImages.Query());
        }

        public async Task<Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetImage.Query, Result>(new GetImage.Query() { ID = id });
        }

        public async Task<int> Create(string filePath)
        {
            return await _commandDispatcher.Dispatch<CreateImage.Command, int>(new CreateImage.Command
            {
                _data = new CreateImage.Data(filePath)

            });

        }

        public async Task Delete(int id)
        {
            await _commandDispatcher.Dispatch<DeleteImage.Command>(new DeleteImage.Command
            {
                ID = id
            });
        }
    }
}
