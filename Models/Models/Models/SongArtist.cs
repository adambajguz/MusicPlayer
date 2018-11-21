using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Models
{
    public class SongArtist
    {
        public virtual Song Song { get; set; }
        public virtual Artist Artist { get; set; }

    }
}
