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

        public string LengthText
        {
            get
            {
                return Length.ToString(@"mm\:ss"); 
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
