using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Models
{
    public class PlayQueue : BaseEntity<int>
    {
        public virtual Song Song { get; set; }

    }
}
