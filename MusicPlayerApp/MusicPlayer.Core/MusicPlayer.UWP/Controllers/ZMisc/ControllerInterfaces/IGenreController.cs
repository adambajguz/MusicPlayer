using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayer.UWP.Controllers.Genre;

namespace MusicPlayer.UWP.Controllers
{
    public interface IGenreController
    {
        Task Create(string name, string description);
    }
}