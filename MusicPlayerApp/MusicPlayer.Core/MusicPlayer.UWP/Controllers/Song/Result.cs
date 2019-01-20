using System;

namespace MusicPlayer.UWP.Controllers.Song
{
    public class Result : Core.Entities.Song
    {
        public Result(Core.Entities.Song x)
        {
            Id = x.Id;
            ImageId = x.ImageId;
            GenreId = x.GenreId;
            Score = x.Score;
            Title = x.Title;
            CreationDate = x.CreationDate;
            FilePath = x.FilePath;
            Length = x.Length;
            bitrate = x.bitrate;
            DBCreationDate = x.DBCreationDate;
            PlayTimes = x.PlayTimes;
        }

        private Result()
        {
            Id = 100000;
            ImageId = 100000;
            GenreId = 100000;
            Score = 3;
            Title = "tytul";
            CreationDate = DateTime.UtcNow;
            FilePath = "/path";
            Length = TimeSpan.FromSeconds(120);
            bitrate = 120;
            DBCreationDate = DateTime.UtcNow;
            PlayTimes = 0;
        }

        public string LengthText
        {
            get
            {
                return "Length: " + Length.ToString(@"mm\:ss"); 
            }
        }


        public string PlayTimesText
        {
            get
            {
                return PlayTimes + " times played";
            }
        }
    }
}
