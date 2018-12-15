using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Entities
{
    public class SongPlaylist : BaseEntity<int>
    {
        public int SongId { get; set; }
        public virtual Song Song { get; set; }
        public int PlaylistId { get; set; }
        public virtual Playlist Playlist { get; set; }

        public int Order { get; set; }

    }
}
