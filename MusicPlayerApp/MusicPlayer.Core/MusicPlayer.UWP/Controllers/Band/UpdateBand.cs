using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Band
{
    public class UpdateBand
    {
        public class Command : ICommand
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public DateTime CreationDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Description { get; set; }
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
                var band = _uow.BandRepository.Query().Where(x => x.Id == command.ID).First();
                band.name = command.Name;
                band.CreationData = command.CreationDate;
                band.EndDate = command.EndDate;
                band.Description = command.Description;

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

