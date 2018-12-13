using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.Data;
using MusicPlayer.Core.Entities;
using System;
using System.Linq;

namespace MusicPlayer.Data
{
    public class EntityRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>, new()
        where TKey : IComparable
    {
        private readonly DbContext _context;

        private bool _disposed;

        public EntityRepository(DbContext context)
        {
            _context = context;
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
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
            }

            _disposed = true;
        }

        public void Insert(TEntity entity)
        {
            _context.Add(entity);
        }

        public IQueryable<TEntity> Query()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }
    }
}