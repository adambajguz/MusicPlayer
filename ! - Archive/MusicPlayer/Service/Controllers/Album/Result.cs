namespace MusicPlayer.Service.Controllers.Album
{
    public class Result : Core.Entities.Album
    {
        public Result(Core.Entities.Album x)
        {
            Id = x.Id;
            CoverImage = x.CoverImage;
            Title = x.Title;
            Description = x.Description;
            PublicationDate = x.PublicationDate;
            DBCreationDate = x.DBCreationDate;
        }

    }
}
