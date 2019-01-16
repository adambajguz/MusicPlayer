using System;

namespace MusicPlayer.UWP.Pages.Artists
{
    public class ArtistData
    {
        public Controllers.Artist.Result Artist { get; set; }

        public String Author
        {
            get
            {
                String pseudo = Artist.Pseudonym != "" ? " '" + Artist.Pseudonym + "' " : " ";
                return Artist.Name + pseudo + Artist.Surname;
            }
        }


        public String BandName { get; set; }

        public String BandNameInBrackets
        {
            get
            {
                if (BandName == "")
                    return "";

                return "(" + BandName + ")";
            }
        }        

        public ArtistData(Controllers.Artist.Result artist, String bandName)
        {
            this.Artist = artist;
            this.BandName = bandName;
        }
    }
}
