using System.Threading.Tasks;

namespace MusicPlayer.Core.CQRS
{
    public interface ICommandDispatcher
    {
        Task<TResult> Dispatch<TCommand, TResult>(TCommand command) where TCommand : ICommand;

        Task Dispatch<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
