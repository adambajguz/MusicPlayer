using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers.Image
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
                _uow.ImageRepository.Insert(command._data.ToImageEntity());
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
            public string FilePath { get; set; }

            public Data(string filePath)
            {
                this.FilePath = filePath;
            }

            public Core.Entities.Image ToImageEntity()
            {
                var image = new Core.Entities.Image
                {
                    FilePath = FilePath
                };
                return image;
            }


        }
    }
}
