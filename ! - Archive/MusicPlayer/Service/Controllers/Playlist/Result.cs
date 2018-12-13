namespace MusicPlayer.Service.Controllers.Playlist
{
    public class Result : Core.Entities.Playlist
    {
        public Result(Core.Entities.Playlist x)
        {
            Id = x.Id;
            Name = x.Name;
            Description = x.Description;
            DBCreationDate = x.DBCreationDate;
        }

    }
}
