using System;
using System.Collections.Generic;

namespace MusicPlayer.Core.Entities
{
    public class Album : BaseEntity<int>
    {
        public int ImageId { get; set; }
        public virtual Image Image { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime PublicationDate { get; set; }

        public DateTime DBCreationDate { get; set; }

        public ICollection<SongAlbum> SongAlbums { get; set; }
    }
}
