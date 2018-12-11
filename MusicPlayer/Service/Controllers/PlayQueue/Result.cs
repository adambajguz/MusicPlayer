namespace MusicPlayer.Service.Controllers.PlayQueue
{
    public class Result : Core.Entities.PlayQueue
    {
        public Result(Core.Entities.PlayQueue x)
        {
            Id = x.Id;
            Song = x.Song;
        }

    }
}
