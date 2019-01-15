﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayer.UWP.Pages.Songs
{
    public class SongData
    {
        public Controllers.Song.Result Song { get; set; }

        private List<Controllers.Album.Result> Albums;
        private List<Controllers.Artist.Result> Artists;

        public String SongAlbums
        {
            get
            {
                String str = "";

                var last = Albums.Last();
                foreach (var album in Albums)
                {
                    str += album.Title;

                    if (!album.Equals(last))
                        str += ", ";
                }

                return str;
            }
        }


        public String SongArtists
        {
            get
            {
                String str = "";

                var last = Artists.Last();
                foreach (var artist in Artists)
                {
                    str += artist.Name + " " + artist.Surname;

                    if (!artist.Equals(last))
                        str += ", ";
                }

                return str;
            }
        }


    
        public SongData(Controllers.Song.Result artist, List<Controllers.Album.Result> albums, List<Controllers.Artist.Result> artists)
        {
            this.Song = artist;
            this.Albums = albums;
            this.Artists = artists;
        }
    }
}
