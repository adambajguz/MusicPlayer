using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.UWP.Controllers.ZMisc;
using System;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Genre
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
                _uow.GenreRepository.Insert(command._data.GetEntity());
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

        // Private Class Data Pattern
        public class Data : ICreateResultData<Core.Entities.Genre>
        {
            private Core.Entities.Genre genre;

            public Data(string name, string description)
            {
                genre = new Core.Entities.Genre();

                genre.Name = name;
                genre.Description = description;
            }

            public Core.Entities.Genre GetEntity()
            {
                return genre;
            }
        }
    }
}

