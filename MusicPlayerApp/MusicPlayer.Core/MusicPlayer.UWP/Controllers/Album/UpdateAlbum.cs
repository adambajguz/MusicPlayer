using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Album
{
    public class UpdateAlbum
    {
        public class Command : ICommand
        {
            public int ID { get; set; }
            public int ImageId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime PublicationDate { get; set; }
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
                var album = _uow.AlbumRepository.Query().Where(x => x.Id == command.ID).First();
                album.ImageId = command.ImageId;
                album.Title = command.Title;
                album.Description = command.Description;
                album.PublicationDate = command.PublicationDate;

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
