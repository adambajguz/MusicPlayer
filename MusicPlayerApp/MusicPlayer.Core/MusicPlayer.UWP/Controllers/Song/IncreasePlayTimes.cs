using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.UWP.Controllers.ZMisc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Song
{
    public class IncreasePlayTimes
    {
        public class Command : ICommand
        {
            public int ID { get; set; }
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
                var song = _uow.SongRepository.Query().Where(x => x.Id == command.ID).First();
                ++song.PlayTimes;
                int i = _uow.SaveChanges();
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


    }
}

