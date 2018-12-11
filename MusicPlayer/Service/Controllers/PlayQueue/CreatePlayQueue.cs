using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers.PlayQueue
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
                _uow.PlayQueueRepository.Insert(command._data.PlayQueue);
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


        public class Data
        {
            public Core.Entities.PlayQueue PlayQueue { get; private set; }

            public Data()
            {
                PlayQueue = new Core.Entities.PlayQueue();

                //PlayQueue.Song = song;
            }
        }
    }
}

