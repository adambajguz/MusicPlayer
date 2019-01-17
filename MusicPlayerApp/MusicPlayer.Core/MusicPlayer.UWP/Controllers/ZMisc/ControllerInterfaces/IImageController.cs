using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayer.UWP.Controllers.Image;

namespace MusicPlayer.UWP.Controllers
{
    public interface IImageController
    {
        Task<int> Create(string filePath);
    }
}