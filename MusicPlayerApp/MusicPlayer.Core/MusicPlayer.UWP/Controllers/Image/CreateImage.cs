using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.UWP.Controllers.ZMisc;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Image
{
    public class CreateImage
    {
        public class Command : ICommand
        {
            public Data _data { get; set; }
        }

        public class Handler : ICommandHandler<Command, int>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<int> Execute(Command command)
            {
                _uow.ImageRepository.Insert(command._data.GetEntity());
                int i = _uow.SaveChanges();
                await _uow.SaveChangesAsync();

                return command._data.GetEntity().Id;
            }
        }

        public class Validator : AbstractValidator<Command>
        {

            public Validator()
            {
                //RuleFor(x => x._data.Value).GreaterThan(0);
            }
        }

        // Private Class Data Pattern
        public class Data : ICreateResultData<Core.Entities.Image>
        {
            private Core.Entities.Image image;

            public Data(string filePath)
            {
                image = new Core.Entities.Image();

                image.FilePath = filePath;
            }

            public Core.Entities.Image GetEntity()
            {
                return image;
            }
        }
    }
}
