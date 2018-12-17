using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.UWP.Controllers.ZMisc;
using System;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Album
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
                _uow.AlbumRepository.Insert(command._data.GetEntity());
                int i = _uow.SaveChanges();
                await _uow.SaveChangesAsync();
            }
        }

        public class Validator : AbstractValidator<Command>
        {

            public Validator()
            {
                RuleFor(x => x._data.GetEntity().Title).NotEmpty();
                RuleFor(x => x._data.GetEntity().ImageId).NotNull();
                RuleFor(x => x._data.GetEntity().PublicationDate).NotEmpty();
            }
        }

        // Private Class Data Pattern
        public class Data : ICreateResultData<Core.Entities.Album>
        {
            private Core.Entities.Album album;

            public Data(string title, string description, DateTime publicationDate, int imageId)
            {
                album = new Core.Entities.Album();

                album.ImageId = imageId;
                album.Title = title;
                album.Description = description;
                album.PublicationDate = publicationDate;
                album.DBCreationDate = DateTime.UtcNow;
            }

            public Core.Entities.Album GetEntity()
            {
                return album;
            }
        }
    }
}

