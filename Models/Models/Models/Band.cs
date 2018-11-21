using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Models
{
    public class Band : BaseEntity<int>
    {
        public string name { get; set; }

        public DateTime CreationData { get; set; }
        public DateTime? EndDate { get; set; }

        public string Description { get; set; }

    }
}
