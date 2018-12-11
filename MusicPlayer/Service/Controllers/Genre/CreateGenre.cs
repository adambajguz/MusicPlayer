using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers.Genre
{
    public class CreateGenre
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
                _uow.GenreRepository.Insert(command._data.Genre);
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
            public Core.Entities.Genre Genre { get; private set; }

            public Data(string name, string description)
            {
                Genre = new Core.Entities.Genre();

                Genre.Name = name;
                Genre.Description = description;
            }
        }
    }
}

