using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.PlayQueue
{
    public class CreatePlayQueue
    {
        public class Command : ICommand
        {
            public Data _data { get; set; }
        }

        public class Handler : ICommandHandler<Command>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task Execute(Command command)
            {
                _uow.PlayQueueRepository.Insert(command._data.GetEntity());
                await _uow.SaveChangesAsync();
            }
        }

        public class Validator : AbstractValidator<Command>
        {

            public Validator()
            {
                //RuleFor(x => x._data.Value).GreaterThan(0);
            }
        }

        // Private Class Data Pattern
        public class Data : ICreateResultData<Core.Entities.PlayQueue>
        {
            private Core.Entities.PlayQueue playQueue;

            public Data()
            {
                playQueue = new Core.Entities.PlayQueue();

                //playQueue.Song = song;
            }

            public Core.Entities.PlayQueue GetEntity()
            {
                return playQueue;
            }
        }
    }
}

