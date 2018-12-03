using System;

namespace MusicPlayer.Core.Entities
{
    public class Genre : BaseEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
