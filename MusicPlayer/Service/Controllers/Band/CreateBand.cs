using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers.Band
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
                _uow.BandRepository.Insert(command._data.ToBandEntity());
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
            public string name { get; set; }
            public DateTime CreationData { get; set; }
            public DateTime? EndDate { get; set; }
            public string Description { get; set; }

            public Data(string name, DateTime creationData, DateTime? endDate, string description)
            {
                this.name = name;
                this.CreationData = creationData;
                this.EndDate = endDate;
                this.Description = description;
            }

            public Core.Entities.Band ToBandEntity()
            {
                var Band = new Core.Entities.Band
                {
                    name = name,
                    CreationData=CreationData,
                    EndDate=EndDate,
                    Description=Description
                };
                return Band;
            }


        }
    }
}

