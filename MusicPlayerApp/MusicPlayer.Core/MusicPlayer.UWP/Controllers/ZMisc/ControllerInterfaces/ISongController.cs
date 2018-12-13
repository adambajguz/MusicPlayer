using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayer.UWP.Controllers.Song;

namespace MusicPlayer.UWP.Controllers
{
    public interface ISongController
    {
        Task Create(int score, string title, DateTime creationDate, string filePath);
    }
}