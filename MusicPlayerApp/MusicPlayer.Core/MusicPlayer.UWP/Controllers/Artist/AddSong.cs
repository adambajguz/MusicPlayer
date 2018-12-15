using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.UWP.Controllers.ZMisc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Artist
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
                _uow.SongArtistRepository.Insert(command._data.GetEntity());
                int i = _uow.SaveChanges();
                await _uow.SaveChangesAsync();
            }
        }

        public class Validator : AbstractValidator<Command>
        {

            public Validator()
            {
                RuleFor(x => x._data.GetEntity().SongId).NotNull();
                RuleFor(x => x._data.GetEntity().Artist).NotNull();
            }
        }

        // Private Class Data Pattern
        public class Data : ICreateResultData<Core.Entities.SongArtist>
        {
            private Core.Entities.SongArtist songArtist;

            public Data(int artistId, int songId)
            {
                songArtist = new Core.Entities.SongArtist();

                songArtist.ArtistId = artistId;
                songArtist.SongId = songId;
            }

            public Core.Entities.SongArtist GetEntity()
            {
                return songArtist;
            }
        }
    }
}



