namespace MusicPlayer.Service.Controllers.Artist
{
    public class Result : Core.Entities.Artist
    {
        public Result(Core.Entities.Artist x)
        {
            Id = x.Id;
            Band = x.Band;
            Photo = x.Photo;
            Name = x.Name;
            Surname = x.Surname;
            Pseudonym = x.Pseudonym;
            Birthdate = x.Birthdate;
            Description = x.Description;
        }
    }
}
