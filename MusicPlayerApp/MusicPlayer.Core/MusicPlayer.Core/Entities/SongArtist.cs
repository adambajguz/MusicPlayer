using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Entities
{
    public class SongArtist : BaseEntity<int>
    {
        public int SongId { get; set; }
        public virtual Song Song { get; set; }
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }


    }
}
