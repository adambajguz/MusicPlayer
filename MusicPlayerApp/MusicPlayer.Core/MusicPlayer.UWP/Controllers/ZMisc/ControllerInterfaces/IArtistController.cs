using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayer.UWP.Controllers.Artist;

namespace MusicPlayer.UWP.Controllers
{
    public interface IArtistController
    {
        Task Create(string name, string surname, string pseudonym, DateTime birthdate, string description, int? bandId, int imageId);
    }
}