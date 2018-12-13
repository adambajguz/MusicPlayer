using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Entities
{
    public class Song : BaseEntity<int>
    {
        public virtual Artist Artist { get; set; }

        public virtual Album Album { get; set; }

        public virtual Image Image { get; set; }

        public virtual Genre Genre { get; set; }

        public int Score { get; set; }

        public string Title { get; set; }

        public DateTime CreationDate { get; set; }

        public string FilePath { get; set; }

        public TimeSpan Length { get; set; }

        public uint bitrate { get; set; }

        public DateTime DBCreationDate { get; set; }

        public uint PlayTimes { get; set; } 
    }
}
