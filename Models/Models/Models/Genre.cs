using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Models
{
    public class Genre : BaseEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
