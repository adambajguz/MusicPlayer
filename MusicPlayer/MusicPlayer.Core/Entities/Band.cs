using System;

namespace MusicPlayer.Core.Entities
{
    public class Band : BaseEntity<int>
    {
        public string name { get; set; }

        public DateTime CreationData { get; set; }
        public DateTime? EndDate { get; set; }

        public string Description { get; set; }

    }
}
