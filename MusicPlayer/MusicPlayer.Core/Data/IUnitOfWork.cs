using MusicPlayer.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Album, int> AlbumRepository { get; }
        IRepository<Artist, int> ArtistRepository { get; }
        IRepository<Band, int> BandRepository { get; }
        IRepository<Genre, int> GenreRepository { get; }
        IRepository<Image, int> ImageRepository { get; }
        IRepository<Playlist, int> PlaylistRepository { get; }
        IRepository<PlayQueue, int> PlayQueueRepository { get; }
        IRepository<Song, int> SongRepository { get; }
        IRepository<SongAlbum, int> SongAlbumRepository { get; }
        IRepository<SongArtist, int> SongArtistRepository { get; }
        IRepository<SongPlaylist, int> SongPlaylistRepository { get; }

        void BeginTransaction();

        int Commit();

        Task<int> CommitAsync();

        void Dispose(bool disposing);

        IRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey> where TKey : IComparable;

        void Rollback();

        int SaveChanges();

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}