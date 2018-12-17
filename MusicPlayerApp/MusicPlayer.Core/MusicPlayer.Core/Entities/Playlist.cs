using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Entities
{
    public class Playlist : BaseEntity<int>
    {
        public string Name { get; set; }

        public DateTime DBCreationDate { get; set; }

        public string Description { get; set; }

        public ICollection<SongPlaylist> SongPlaylists { get; set; }
    }
}
