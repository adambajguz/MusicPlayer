using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers.Playlist
{
    public class CreatePlaylist
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
                _uow.PlaylistRepository.Insert(command._data.Playlist);
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
            public Core.Entities.Playlist Playlist { get; private set; }

            public Data(string name, string description)
            {
                Playlist = new Core.Entities.Playlist();

                Playlist.Name = name;
                Playlist.Description = description;
                Playlist.DBCreationDate = DateTime.UtcNow;
            }
        }
    }
}

