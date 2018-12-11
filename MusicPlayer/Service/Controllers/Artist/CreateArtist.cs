using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers.Artist
{
    public class CreateArtist
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
                _uow.ArtistRepository.Insert(command._data.Artist);
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
            public Core.Entities.Artist Artist { get; private set; }

            public Data(string name, string surname, string pseudonym, DateTime birthdate, string description)
            {
                Artist = new Core.Entities.Artist();

                //Artist.Band = x.Band;
                //Artist.Photo = x.Photo;
                Artist.Name = name;
                Artist.Surname = surname;
                Artist.Pseudonym = pseudonym;
                Artist.Birthdate = birthdate;
                Artist.Description = description;
            }
        }
    }
}

