using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.UWP.Controllers.ZMisc;
using System;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Playlist
{
    public class AddSong
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
                _uow.SongPlaylistRepository.Insert(command._data.GetEntity());
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
        public class Data : ICreateResultData<Core.Entities.SongPlaylist>
        {
            private Core.Entities.SongPlaylist songPlaylist;

            public Data(int playlistId, int songId)
            {
                songPlaylist = new Core.Entities.SongPlaylist();

                songPlaylist.PlaylistId = playlistId;
                songPlaylist.SongId = songId;
            }

            public Core.Entities.SongPlaylist GetEntity()
            {
                return songPlaylist;
            }
        }
    }
}

