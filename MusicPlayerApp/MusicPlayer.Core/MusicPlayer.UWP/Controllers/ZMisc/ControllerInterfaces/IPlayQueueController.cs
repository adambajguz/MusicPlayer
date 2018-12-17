using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayer.UWP.Controllers.PlayQueue;

namespace MusicPlayer.UWP.Controllers
{
    public interface IPlayQueueController
    {
        Task Create(int songId);
    }
}