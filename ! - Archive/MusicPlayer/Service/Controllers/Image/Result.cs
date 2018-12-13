namespace MusicPlayer.Service.Controllers.Image
{
    public class Result : Core.Entities.Image
    {
        public Result(Core.Entities.Image x)
        {
            Id = x.Id;
            FilePath = x.FilePath;
        }

    }
}
