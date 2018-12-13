using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.UWP.Controllers.ZMisc;
using System;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Playlist
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
                _uow.PlaylistRepository.Insert(command._data.GetEntity());
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
        public class Data : ICreateResultData<Core.Entities.Playlist>
        {
            private Core.Entities.Playlist playlist;

            public Data(string name, string description)
            {
                playlist = new Core.Entities.Playlist();

                playlist.Name = name;
                playlist.Description = description;
                playlist.DBCreationDate = DateTime.UtcNow;
            }

            public Core.Entities.Playlist GetEntity()
            {
                return playlist;
            }
        }
    }
}

