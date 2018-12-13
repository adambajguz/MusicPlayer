using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.UWP.Controllers.ZMisc;
using System;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Song
{
    public class CreateSong
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
                _uow.SongRepository.Insert(command._data.GetEntity());
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
        public class Data : ICreateResultData<Core.Entities.Song>
        {
            private Core.Entities.Song song;

            public Data(int score, string title, DateTime creationDate, string filePath)
            {
                song = new Core.Entities.Song();

                //song.Artist = Artist;
                //song.Album = Album;
                //song.Image = Image;
                //song.Genre = Genre;
                song.Score = score;
                song.Title = title;
                song.CreationDate = creationDate;
                song.FilePath = filePath;
                song.Length = new TimeSpan(0, 0, 0); // get from file if exists
                song.bitrate = 0;  // get from file if exists
                song.DBCreationDate = DateTime.UtcNow;
                song.PlayTimes = 0;
            }

            public Core.Entities.Song GetEntity()
            {
                return song;
            }
        }
    }
}

