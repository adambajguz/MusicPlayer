namespace MusicPlayer.UWP.Controllers.Song
{
    public class Result : Core.Entities.Song
    {
        public Result(Core.Entities.Song x)
        {
            Id = x.Id;
            Artist = x.Artist;
            Album = x.Album;
            Image = x.Image;
            Genre = x.Genre;
            Score = x.Score;
            Title = x.Title;
            CreationDate = x.CreationDate;
            FilePath = x.FilePath;
            Length = x.Length;
            bitrate = x.bitrate;
            DBCreationDate = x.DBCreationDate;
            PlayTimes = x.PlayTimes;
        }

    }
}
