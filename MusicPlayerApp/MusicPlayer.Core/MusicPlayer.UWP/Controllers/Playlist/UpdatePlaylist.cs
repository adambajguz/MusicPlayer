using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Playlist
{
    public class UpdatePlaylist
    {
        public class Command : ICommand
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
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
                var playlist = _uow.PlaylistRepository.Query().Where(x => x.Id == command.ID).First();
                playlist.Name = command.Name;
                playlist.Description = command.Description;

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


