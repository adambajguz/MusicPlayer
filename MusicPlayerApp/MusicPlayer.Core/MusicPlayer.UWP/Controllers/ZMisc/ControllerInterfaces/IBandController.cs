using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayer.UWP.Controllers.Band;

namespace MusicPlayer.UWP.Controllers
{
    public interface IBandController
    {
        Task Create(string name, DateTime creationDate, DateTime? endDate, string description);
    }
}