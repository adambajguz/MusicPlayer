using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Song
{
    public class UpdateSong
    {
        public class Command : ICommand
        {
            public int ID { get; set; }
           public int GenreId { get; set; }
            public int? ImageId { get; set; }
            public int Score { get; set; }
            public string Title { get; set; }
            public DateTime CreationDate { get; set; }
            public string FilePath { get; set; }
            public TimeSpan Length { get; set; }
            public uint Bitrate { get; set; }
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
                var song = _uow.SongRepository.Query().Where(x => x.Id == command.ID).First();
                song.GenreId = command.GenreId;
                song.ImageId = command.ImageId;
                song.Score = command.Score;
                song.Title = command.Title;
                song.CreationDate = command.CreationDate;
                song.FilePath = command.FilePath;
                song.Length = command.Length;
                song.bitrate = command.Bitrate;


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


