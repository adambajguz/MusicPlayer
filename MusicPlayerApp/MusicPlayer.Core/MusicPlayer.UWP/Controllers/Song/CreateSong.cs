using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
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
                _uow.SongRepository.Insert(command._data.Song);
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
        public class Data
        {
            public Core.Entities.Song Song { get; private set; }

            public Data(int score, string title, DateTime creationDate, string filePath)
            {
                Song = new Core.Entities.Song();

                //Song.Artist = Artist;
                //Song.Album = Album;
                //Song.Image = Image;
                //Song.Genre = Genre;
                Song.Score = score;
                Song.Title = title;
                Song.CreationDate = creationDate;
                Song.FilePath = filePath;
                Song.Length = new TimeSpan(0, 0, 0); // get from file if exists
                Song.bitrate = 0;  // get from file if exists
                Song.DBCreationDate = DateTime.UtcNow;
                Song.PlayTimes = 0;
            }
        }
    }
}

