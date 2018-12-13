using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.UWP.Controllers.ZMisc;
using System;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Band
{
    public class CreateBand
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
                _uow.BandRepository.Insert(command._data.GetEntity());
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

        // Private Class Data Pattern
        public class Data : ICreateResultData<Core.Entities.Band>
        {
            private Core.Entities.Band band;

            //public string name { get; set; }
            //public DateTime CreationData { get; set; }
            //public DateTime? EndDate { get; set; }
            //public string Description { get; set; }

            public Data(string name, DateTime creationData, DateTime? endDate, string description)
            {
                band = new Core.Entities.Band();

                band.name = name;
                band.CreationData = creationData;
                band.EndDate = endDate;
                band.Description = description;
            }

            public Core.Entities.Band GetEntity()
            {
                return band;
            }

            //public Core.Entities.Band ToBandEntity()
            //{
            //    var Band = new Core.Entities.Band
            //    {
            //        name = name,
            //        CreationData=CreationData,
            //        EndDate=EndDate,
            //        Description=Description
            //    };
            //    return Band;
            //}
        }
    }
}

