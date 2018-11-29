using MusicPlayer.Core.Entities;
using System;
using System.Linq;

namespace MusicPlayer.Core.Data
{
    public interface IRepository<TEntity, TKey> : IDisposable
        where TEntity : BaseEntity<TKey>
        where TKey : IComparable
    {
        void Delete(TEntity entity);

        void Insert(TEntity entity);

        IQueryable<TEntity> Query();

        void Update(TEntity entity);
    }
}