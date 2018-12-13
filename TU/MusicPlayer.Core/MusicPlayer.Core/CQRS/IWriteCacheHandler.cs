using System;
using System.Threading.Tasks;

namespace MusicPlayer.Core.CQRS
{
    public interface IWriteCacheHandler<TQuery, TResult> : ICacheKey<TQuery>
    {
        Task Save(TQuery query, TResult result, TimeSpan? ttl = null);
    }
}