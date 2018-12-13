using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayer.UWP.Controllers.Playlist;

namespace MusicPlayer.UWP.Controllers
{
    public interface IPlaylistController
    {
        Task Create(string name, string description);
    }
}