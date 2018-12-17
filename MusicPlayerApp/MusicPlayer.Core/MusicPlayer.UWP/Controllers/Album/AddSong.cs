using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.UWP.Controllers.ZMisc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Album
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
                    _uow.SongAlbumRepository.Insert(command._data.GetEntity());
                    int i = _uow.SaveChanges();
                    await _uow.SaveChangesAsync();
                }
            }

            public class Validator : AbstractValidator<Command>
            {

                public Validator()
                {
                    RuleFor(x => x._data.GetEntity().SongId).NotNull();
                    RuleFor(x => x._data.GetEntity().AlbumId).NotNull();
                    RuleFor(x => x._data.GetEntity().TrackNumber).GreaterThan(0);
                }
            }

            // Private Class Data Pattern
            public class Data : ICreateResultData<Core.Entities.SongAlbum>
            {
                private Core.Entities.SongAlbum songAlbum;

                public Data(int albumId, int songId, int trackNumber)
                {
                    songAlbum = new Core.Entities.SongAlbum();

                    songAlbum.AlbumId = albumId;
                    songAlbum.SongId = songId;
                    songAlbum.TrackNumber = trackNumber;
                }

                public Core.Entities.SongAlbum GetEntity()
                {
                    return songAlbum;
                }
            }
        }
    }



