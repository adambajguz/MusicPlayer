using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers.Album
{
    public class CreateAlbum
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
                _uow.AlbumRepository.Insert(command._data.Album);
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
            public Core.Entities.Album Album { get; private set; }

            public Data(string title, string description, DateTime publicationDate)
            {
                Album = new Core.Entities.Album();

                //Album.CoverImage = x.CoverImage;
                Album.Title = title;
                Album.Description = description;
                Album.PublicationDate = publicationDate;
                Album.DBCreationDate = DateTime.UtcNow;
            }
        }
    }
}

