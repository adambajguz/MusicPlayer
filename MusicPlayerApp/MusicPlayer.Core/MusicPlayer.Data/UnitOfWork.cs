using MusicPlayer.Core.Data;
using MusicPlayer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.Extensions;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using MusicPlayer.Core.Logging;

namespace MusicPlayer.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        private bool _disposed;

        private Hashtable _repositories;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public IRepository<Album, int> AlbumRepository
        {
            get
            {
                return Repository<Album, int>();
            }
        }

        public IRepository<Artist, int> ArtistRepository
        {
            get
            {
                return Repository<Artist, int>();
            }
        }

        public IRepository<Image, int> ImageRepository
        {
            get
            {
                return Repository<Image, int>();
            }
        }
        public IRepository<Genre, int> GenreRepository
        {
            get
            {
                return Repository<Genre, int>();
            }
        }

        public IRepository<Band, int> BandRepository
        {
            get
            {
                return Repository<Band, int>();
            }
        }

        public IRepository<Playlist, int> PlaylistRepository
        {
            get
            {
                return Repository<Playlist, int>();
            }
        }

        public IRepository<PlayQueue, int> PlayQueueRepository
        {
            get
            {
                return Repository<PlayQueue, int>();
            }
        }

        public IRepository<Song, int> SongRepository
        {
            get
            {
                return Repository<Song, int>();
            }
        }

        public IRepository<SongAlbum, int> SongAlbumRepository
        {
            get
            {
                return Repository<SongAlbum, int>();
            }
        }

        public IRepository<SongArtist, int> SongArtistRepository
        {
            get
            {
                return Repository<SongArtist, int>();
            }
        }

        public IRepository<SongPlaylist, int> SongPlaylistRepository
        {
            get
            {
                return Repository<SongPlaylist, int>();
            }
        }
       

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
                if (_repositories.IsNotNull())
                {
                    foreach (IDisposable repository in _repositories.Values)
                    {
                        repository.Dispose();
                    }
                }
            }

            _disposed = true;
        }

        public IRepository<TEntity, TKey> Repository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
            where TKey : IComparable
        {
            if (_repositories.IsNull())
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
            {
                return (IRepository<TEntity, TKey>)_repositories[type];
            }

            var repositoryType = typeof(EntityRepository<,>);
            _repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(new Type[] { typeof(TEntity), typeof(TKey) }), _context));
            return (IRepository<TEntity, TKey>)_repositories[type];
        }

        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch(Exception e)
            {
                NLogLogger.Instance.Error(e.Message, e.StackTrace);
                throw e;
            }
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}