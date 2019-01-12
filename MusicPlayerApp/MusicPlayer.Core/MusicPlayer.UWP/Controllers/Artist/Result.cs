namespace MusicPlayer.UWP.Controllers.Artist
{
    public class Result : Core.Entities.Artist
    {
        public Result(Core.Entities.Artist x)
        {
            Id = x.Id;
            BandId = x.BandId;
            ImageId = x.ImageId;
            Name = x.Name;
            Surname = x.Surname;
            Pseudonym = x.Pseudonym;
            Birthdate = x.Birthdate;
            Description = x.Description;
            SongArtists = x.SongArtists;

        }
    }
}
