namespace MusicPlayer.UWP.Controllers.Genre
{
    public class Result : Core.Entities.Genre
    {
        public Result(Core.Entities.Genre x)
        {
            Id = x.Id;
            Name = x.Name;
            Description = x.Description;
        }

    }
}
