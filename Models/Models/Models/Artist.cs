using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Models
{
    public class Artist : BaseEntity<int>
    {
        public virtual Band Band { get; set; }
        public virtual Image Photo { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

        public string Pseudonym { get; set; }

        public DateTime Birthdate { get; set; }

        public string Description { get; set; }
    }
}
