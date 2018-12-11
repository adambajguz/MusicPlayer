using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.Entities;

namespace MusicPlayer.Data
{
    public interface IEntitiesContext
    {
        DbSet<Album> Albums { get; set; }
        DbSet<Artist> Artists { get; set; }
        DbSet<Band> Bands { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<Playlist> Playlists { get; set; }
        DbSet<PlayQueue> PlayQueues { get; set; }
        DbSet<Song> Songs { get; set; }
        DbSet<SongAlbum> SongAlbums { get; set; }
        DbSet<SongArtist> SongArtists { get; set; }
        DbSet<SongPlaylist> SongPlaylists { get; set; }

    }
}