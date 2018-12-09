using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Controllers.Band
{
    public class DeleteBand
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
                var band = _uow.BandRepository.Query().Where(x => x.Id == command._data.Id).FirstOrDefault();
                _uow.BandRepository.Delete(band);
                await _uow.SaveChangesAsync();
            }
        }

        public class Data
        {
            public int Id { get; set; }
            public Data(int id)
            {
                this.Id = id;
            }
        }
    }
}

