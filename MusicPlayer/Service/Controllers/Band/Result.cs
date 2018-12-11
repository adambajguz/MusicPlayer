namespace MusicPlayer.Service.Controllers.Band
{
    public class Result : Core.Entities.Band
    {
        public Result(Core.Entities.Band x)
        {
            Id = x.Id;
            name = x.name;
            CreationData = x.CreationData;
            EndDate = x.EndDate;
            Description = x.Description;
        }

    }
}
