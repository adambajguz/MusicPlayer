namespace MusicPlayer.UWP.Controllers.PlayQueue
{
    public class Result : Core.Entities.PlayQueue
    {
        public Result(Core.Entities.PlayQueue x)
        {
            Id = x.Id;
            SongId = x.SongId;
        }

    }
}
