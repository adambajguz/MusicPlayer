using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Models
{
    public class Playlist : BaseEntity<int>
    {
        public string Name { get; set; }

        public DateTime DBCreationDate { get; set; }

        public string Description { get; set; }
    }
}
