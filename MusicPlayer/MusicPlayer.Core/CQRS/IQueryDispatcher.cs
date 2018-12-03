using System.Threading.Tasks;

namespace MusicPlayer.Core.CQRS
{
    public interface IQueryDispatcher
    {
        Task<TResult> Dispatch<TQuery, TResult>(TQuery query) where TQuery : IQuery;
        //cos sprawdzam
        void WriteInformation(string input);
    }
}
