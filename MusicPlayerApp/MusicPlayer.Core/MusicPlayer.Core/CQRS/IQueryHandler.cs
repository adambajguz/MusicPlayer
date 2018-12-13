using System.Threading.Tasks;

namespace MusicPlayer.Core.CQRS
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery
    {
        Task<TResult> Handle(TQuery query);
    }
}