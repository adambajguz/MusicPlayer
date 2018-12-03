using System;

namespace MusicPlayer.Core.Entities
{
    public class Image : BaseEntity<int>
    {
        public string FilePath { get; set; }
    }
}
