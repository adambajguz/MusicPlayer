using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Image
{
    public class CreateImage
    {
        public class Command : ICommand
        {
            public Data _data { get; set; }
        }

        public class Handler : ICommandHandler<Command>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task Execute(Command command)
            {
                _uow.ImageRepository.Insert(command._data.Image);
                int i = _uow.SaveChanges();
                await _uow.SaveChangesAsync();
                
                
            }
        }

        public class Validator : AbstractValidator<Command>
        {

            public Validator()
            {
                //RuleFor(x => x._data.Value).GreaterThan(0);
            }
        }


        public class Data
        {
            public Core.Entities.Image Image { get; private set; }

            public Data(string filePath)
            {
                Image = new Core.Entities.Image();

                Image.FilePath = filePath;
            }
        }
    }
}
