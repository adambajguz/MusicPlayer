using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Entities
{
    public class SongAlbum : BaseEntity<int>
    {
        public virtual Song Song { get; set; }
        public virtual Album Album { get; set; }

        public int TrackNumber { get; set; }
    }
}
