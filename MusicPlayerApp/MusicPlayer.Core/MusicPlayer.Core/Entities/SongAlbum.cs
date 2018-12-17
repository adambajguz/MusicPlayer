using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Entities
{
    public class SongAlbum : BaseEntity<int>
    {
        public int SongId { get; set; }
        public virtual Song Song { get; set; }
        public int AlbumId { get; set; }
        public virtual Album Album { get; set; }

        public int TrackNumber { get; set; }

    }
}
