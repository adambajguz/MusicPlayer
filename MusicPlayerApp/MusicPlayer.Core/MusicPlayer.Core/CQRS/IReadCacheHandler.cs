using System.Threading.Tasks;

namespace MusicPlayer.Core.CQRS
{
    public interface IReadCacheHandler<TQuery, TResult> : ICacheKey<TQuery>
    {
        Task<TResult> Read(TQuery query);
    }
}