using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Artist
{
    public class UpdateArtist
    {
        public class Command : ICommand
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Pseudonym { get; set; }
            public DateTime Birthdate { get; set; }
            public string Description { get; set; }
            public int? BandId { get; set; }
            public int ImageId { get; set; }
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
                var artist = _uow.ArtistRepository.Query().Where(x => x.Id == command.ID).First();
                artist.Name = command.Name;
                artist.Surname = command.Surname;
                artist.Pseudonym = command.Pseudonym;
                artist.Birthdate = command.Birthdate;
                artist.Description = command.Description;
                artist.BandId = command.BandId;
                artist.ImageId = command.ImageId;

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


    }
}
