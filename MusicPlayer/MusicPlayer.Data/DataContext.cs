using MusicPlayer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;

namespace MusicPlayer.Data
{
    public class DataContext : DbContext, IEntitiesContext
    {
        public DbSet<Album> Albums{get;set;}
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
            optionsBuilder.UseSqlServer("Data Source =.\\SQLEXPRESS; Initial Catalog = MusicPlayer; Persist Security Info = False; Integrated Security = True;");
        }

        //public DataContext(DbContextOptions options) : base(options){}
        /*public DataContext(IConfiguration conf )
        {
            _conf = conf;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conf["SQL-Server:localhost"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        */
    }
}
