using MusicPlayer.UWP.Pages.Songs;
using System.Collections.Generic;


namespace MusicPlayer.UWP.Pages.PlayQueue
{
    public class PlayQueueData : SongData {
        public int QueueID { get; set; }


        public PlayQueueData (Controllers.Song.Result song, List<Controllers.Album.Result> albums, List<Controllers.Artist.Result> artists, int queueID) : base(song, albums, artists)
        {
            this.QueueID = queueID;
        }

    }
}
