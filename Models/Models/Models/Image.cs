using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Models
{
    public class Image : BaseEntity<int>
    {
        public String FilePath { get; set; }
    }
}
