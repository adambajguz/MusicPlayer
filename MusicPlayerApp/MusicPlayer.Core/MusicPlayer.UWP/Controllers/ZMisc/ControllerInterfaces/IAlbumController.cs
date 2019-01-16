using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayer.UWP.Controllers.Album;

namespace MusicPlayer.UWP.Controllers
{
    public interface IAlbumController
    {
        Task<int> Create(string title, string description, DateTime publicationDate, int imageId);
    }
}