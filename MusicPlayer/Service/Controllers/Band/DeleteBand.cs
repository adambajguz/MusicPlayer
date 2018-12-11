﻿using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers.Band
{
    public class DeleteBand
    {
        public class Command : ICommand
        {
            public int ID;
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
                var band = _uow.BandRepository.Query().Where(x => x.Id == command.ID).FirstOrDefault();
                _uow.BandRepository.Delete(band);

                await _uow.SaveChangesAsync();
            }
        }
    }
}

