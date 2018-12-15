using System;
using System.Collections.Generic;

namespace MusicPlayer.Core.Entities
{
    public class Artist : BaseEntity<int>
    {
        public int? BandId { get; set; }
        public virtual Band Band { get; set; }
        public int ImageId { get; set; }
        public virtual Image Photo { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

        public string Pseudonym { get; set; }

        public DateTime Birthdate { get; set; }

        public string Description { get; set; }

        public ICollection<SongArtist> SongArtists { get; set; }
    }
}
