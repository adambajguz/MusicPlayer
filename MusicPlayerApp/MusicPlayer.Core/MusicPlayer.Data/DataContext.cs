using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.Entities;

namespace MusicPlayer.Data
{
    public class DataContext : DbContext, IEntitiesContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Band> Bands { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlayQueue> PlayQueues { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<SongAlbum> SongAlbums { get; set; }
        public DbSet<SongArtist> SongArtists { get; set; }
        public DbSet<SongPlaylist> SongPlaylists { get; set; }

        //private readonly IConfiguration _conf;


        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=Music.Player;Integrated Security=SSPI;");
            //optionsBuilder.UseSqlServer("Data Source =.\\SQLEXPRESS; Initial Catalog = MusicPlayer; Persist Security Info = False; Integrated Security = True;");
        }
    }
}
