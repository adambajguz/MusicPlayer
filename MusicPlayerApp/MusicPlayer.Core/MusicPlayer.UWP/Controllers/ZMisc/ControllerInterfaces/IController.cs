using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers
{
    interface IController<T>
    {
        Task<T> Get(int id);

        Task<List<T>> GetAll();

        Task Delete(int id);

    }
}
