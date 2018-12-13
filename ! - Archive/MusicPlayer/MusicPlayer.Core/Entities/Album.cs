using System;

namespace MusicPlayer.Core.Entities
{
    public class Album : BaseEntity<int>
    {
        public virtual Image CoverImage { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime PublicationDate { get; set; }

        public DateTime DBCreationDate { get; set; }
    }
}
